using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using MetricsManager.DAL;
using System.Data.SQLite;
using Dapper;
using AutoMapper;
using FluentMigrator.Runner;
using Quartz;
using Quartz.Spi;
//using MetricsManager.ScheduledWorks;
using Quartz.Impl;
using MetricsManager.SQLsettings;
using MetricsManager.ScheduledWorks;

namespace MetricsManager
{
	public class Startup
	{
		/// <summary>
		/// ������������� ������� ����� �� ����� ������
		/// </summary>
		private const string CronExpression = "0 * * ? * *";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// �����������
			services.AddControllers();

			// �����������
			services.AddSingleton<IAgentsRepository, AgentsRepository>();
			services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();
			//services.AddSingleton<IDotNetMetricsRepository, DotNetMetricsRepository>();
			//services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();
			//services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();
			//services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();

			// ������
			var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
			var mapper = mapperConfiguration.CreateMapper();
			services.AddSingleton(mapper);

			// ��������� ��� ���� ������
			services.AddSingleton<IMySqlSettings, MySqlSettings>();

			// ��������
			services.AddFluentMigratorCore()
				.ConfigureRunner(rb => rb.AddSQLite() // ��������� ��������� SQLite 
					.WithGlobalConnectionString(new MySqlSettings().ConnectionString)// ������������� ������ �����������
					.ScanIn(typeof(Startup).Assembly).For.Migrations())// ������������ ��� ������ ������ � ����������
				.AddLogging(lb => lb.AddFluentMigratorConsole());

			// ��������� ����� ������ �� ����������
			JobsSheduleSettings(services);

			// !DEBUG
			PrepareSchema();

		}

		private void PrepareSchema()
		{
			//! DEBUG �������� ������ �� �������
			var mySql = new MySqlSettings();

			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				connection.ExecuteAsync(
				$"INSERT INTO {mySql.AgentsTable}" +
				$"({mySql[Columns.AgentId]}, {mySql[Columns.AgentUri]})" +
				$"VALUES (@agentId, @agentUri);",
				new
				{
					agentId = 1,
					agentUri = "https://localhost:5001",
				});

				connection.ExecuteAsync(
				$"INSERT INTO {mySql.AgentsTable}" +
				$"({mySql[Columns.AgentId]}, {mySql[Columns.AgentUri]})" +
				$"VALUES (@agentId, @agentUri);",
				new
				{
					agentId = 2,
					agentUri = "https://localhost:5003",
				});

			}

		}

		/// <summary>
		/// ��������� ����� ������ �� ����������
		/// </summary>
		/// <param name="services"></param>
		private void JobsSheduleSettings(IServiceCollection services)
		{
			// ������������ �������
			services.AddSingleton<IJobFactory, SingletonJobFactory>();
			services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

			// ������ ��� ������ ������
			services.AddSingleton<CpuMetricJob>();

			string debugCronExpression = "* 31/5 * ? * * *";

			// ������������� ������� �����
			services.AddSingleton(new JobSchedule(
				jobType: typeof(CpuMetricJob),
				cronExpression: debugCronExpression));

			// ������ ��� ������� ����� � ������� Quarz
			services.AddHostedService<QuartzHostedService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			migrationRunner.MigrateUp();
		}
	}
}
