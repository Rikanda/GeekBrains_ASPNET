using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.DAL;
using System.Data.SQLite;
using Dapper;

namespace MetricsAgent
{
	public class Startup
	{
		//Строка подключения к базе данных
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
			ConfigureSqlLiteConnection(services);
			services.AddScoped<ICpuMetricsRepository, CpuMetricsRepository>();
			services.AddScoped<IDotNetMetricsRepository, DotNetMetricsRepository>();
			services.AddScoped<IHddMetricsRepository, HddMetricsRepository>();
			services.AddScoped<INetworkMetricsRepository, NetworkMetricsRepository>();
			services.AddScoped<IRamMetricsRepository, RamMetricsRepository>();
		}

		private void ConfigureSqlLiteConnection(IServiceCollection services)
		{
			using (var connection = new SQLiteConnection(ConnectionString))
			{
				connection.Open();
				PrepareSchema(connection);
			}
		}

		/// <summary>
		/// Создание базы данных и заполнение ее информацией для тестов
		/// </summary>
		/// <param name="connection">Соединение с базой</param>
		private void PrepareSchema(SQLiteConnection connection)
		{
			using (var command = new SQLiteCommand(connection))
			{
				// имена таблиц
				List<string> tablesNames = new List<string>
				{
					"cpumetrics",
					"dotnetmetrics",
					"hddmetrics",
					"networkmetrics",
					"rammetrics",
				};

				foreach(string name in tablesNames)
				{
					connection.Execute($"DROP TABLE IF EXISTS {name}");
					connection.Execute(@$"CREATE TABLE {name}(id INTEGER PRIMARY KEY, value INT, time INT64)");

					//Забиваем базу данных фигней для тестов
					for (int i = 0; i < 10; i++)
					{
						DateTimeOffset time = new DateTime(2000 + i, 1, 1);
						connection.Execute(@$"INSERT INTO {name}(value, time) VALUES({i*10+tablesNames.IndexOf(name)},{time.ToUnixTimeSeconds()})");
					}
				}
			}
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
		}
	}
}
