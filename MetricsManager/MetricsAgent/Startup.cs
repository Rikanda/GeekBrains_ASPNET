using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using MetricsAgent.DAL;
using System.Data.SQLite;
using Dapper;
using AutoMapper;
using FluentMigrator.Runner;
using Quartz;
using Quartz.Spi;
using MetricsAgent.ScheduledWorks;
using Quartz.Impl;
using MetricsAgent.MySQLsettings;
using MetricsAgent.ScheduledWorks.Tools;
using MetricsAgent.ScheduledWorks.Jobs;
using MetricsAgent.DAL.Repositories;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;

namespace MetricsAgent
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
			services.AddSingleton<IDotNetMetricsRepository, DotNetMetricsRepository>();
			services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();
			services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();
			services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();

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

			// Swagger
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "API for Metrics Agent service",
					Description = "Additional information",
					TermsOfService = new Uri("https://example.com/"),
					Contact = new OpenApiContact
					{
						Name = "Vasiliy Mykitenko",
						Email = string.Empty,
						Url = new Uri("https://example.com/contacts"),
					},
					License = new OpenApiLicense
					{
						Name = "License - ��0",
						Url = new Uri("https://creativecommons.org/choose/zero/"),
					}
				});
				// ��������� ���� �� �������� ����� ����������� ��� Swagger UI
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});
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
			services.AddSingleton<DotNetMetricJob>();
			services.AddSingleton<HddMetricJob>();
			services.AddSingleton<NetworkMetricJob>();
			services.AddSingleton<RamMetricJob>();

			// ������������� ������� �����
			services.AddSingleton(new JobSchedule(
				jobType: typeof(CpuMetricJob),
				cronExpression: CronExpression));
			services.AddSingleton(new JobSchedule(
				jobType: typeof(DotNetMetricJob),
				cronExpression: CronExpression));
			services.AddSingleton(new JobSchedule(
				jobType: typeof(HddMetricJob),
				cronExpression: CronExpression));
			services.AddSingleton(new JobSchedule(
				jobType: typeof(NetworkMetricJob),
				cronExpression: CronExpression));
			services.AddSingleton(new JobSchedule(
				jobType: typeof(RamMetricJob),
				cronExpression: CronExpression));

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

			//app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			migrationRunner.MigrateUp();

			// ��������� middleware � �������� ��� ��������� Swagger ��������.
			app.UseSwagger();
			// ��������� middleware ��� ��������� swagger-ui 
			// ��������� Swagger JSON �������� (���� ���������� �� ��������������� �������������
			// �� ������� ����� �������� UI).
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "API ������� ������ ����� ������");
				c.RoutePrefix = string.Empty;
			});
		}
	}
}
