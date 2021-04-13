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

namespace MetricsManager
{
	public class Startup
	{
		/// <summary>
		/// ������������� ������� ����� �� ����� ������
		/// </summary>
		private const string CronExpression = "0/5 * * * * ?";

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
			//JobsSheduleSettings(services);


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
