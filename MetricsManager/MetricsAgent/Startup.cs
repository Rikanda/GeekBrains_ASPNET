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

namespace MetricsAgent
{
	public class Startup
	{
		/// <summary>
		/// Строка для подключения к базе данных
		/// </summary>
		private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			//ConfigureSqlLiteConnection();

			services.AddScoped<ICpuMetricsRepository, CpuMetricsRepository>();
			services.AddScoped<IDotNetMetricsRepository, DotNetMetricsRepository>();
			services.AddScoped<IHddMetricsRepository, HddMetricsRepository>();
			services.AddScoped<INetworkMetricsRepository, NetworkMetricsRepository>();
			services.AddScoped<IRamMetricsRepository, RamMetricsRepository>();

			var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
			var mapper = mapperConfiguration.CreateMapper();
			services.AddSingleton(mapper);

			services.AddFluentMigratorCore()
				.ConfigureRunner(rb => rb.AddSQLite() // добавляем поддержку SQLite 
					.WithGlobalConnectionString(ConnectionString)// устанавливаем строку подключения
					.ScanIn(typeof(Startup).Assembly).For.Migrations())// подсказываем где искать классы с миграциями
				.AddLogging(lb => lb.AddFluentMigratorConsole());


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
