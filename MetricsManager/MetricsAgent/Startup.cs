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
		/// Периодичность запуска задач по сбору метрик
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
			// Контроллеры
			services.AddControllers();

			// Репозитории
			services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();
			services.AddSingleton<IDotNetMetricsRepository, DotNetMetricsRepository>();
			services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();
			services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();
			services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();

			// Маппер
			var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
			var mapper = mapperConfiguration.CreateMapper();
			services.AddSingleton(mapper);

			// Настройки для базы данных
			services.AddSingleton<IMySqlSettings, MySqlSettings>();

			// Мигратор
			services.AddFluentMigratorCore()
				.ConfigureRunner(rb => rb.AddSQLite() // добавляем поддержку SQLite 
					.WithGlobalConnectionString(new MySqlSettings().ConnectionString)// устанавливаем строку подключения
					.ScanIn(typeof(Startup).Assembly).For.Migrations())// подсказываем где искать классы с миграциями
				.AddLogging(lb => lb.AddFluentMigratorConsole());

			// Настройка сбора метрик по расписанию
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
						Name = "License - СС0",
						Url = new Uri("https://creativecommons.org/choose/zero/"),
					}
				});
				// Указываем файл из которого брать комментарии для Swagger UI
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});
		}

		/// <summary>
		/// Настройка сбора метрик по расписанию
		/// </summary>
		/// <param name="services"></param>
		private void JobsSheduleSettings(IServiceCollection services)
		{
			// Планировщики заданий
			services.AddSingleton<IJobFactory, SingletonJobFactory>();
			services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

			// Задачи для разных метрик
			services.AddSingleton<CpuMetricJob>();
			services.AddSingleton<DotNetMetricJob>();
			services.AddSingleton<HddMetricJob>();
			services.AddSingleton<NetworkMetricJob>();
			services.AddSingleton<RamMetricJob>();

			// Периодичность запуска задач
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

			// Сервис для запуска задач с помощью Quarz
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

			// Включение middleware в пайплайн для обработки Swagger запросов.
			app.UseSwagger();
			// включение middleware для генерации swagger-ui 
			// указываем Swagger JSON эндпоинт (куда обращаться за сгенерированной спецификацией
			// по которой будет построен UI).
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "API сервиса агента сбора метрик");
				c.RoutePrefix = string.Empty;
			});
		}
	}
}
