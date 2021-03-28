using System;
using NLog.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace MetricsManager
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
			try
			{
				logger.Info("---- [BEGIN]----");
				//Samples
				
				CreateHostBuilder(args).Build().Run();
			}
			// отлов всех исключений в рамках работы приложения
			catch (Exception exception)
			{
				//NLog: устанавливаем отлов исключений
				logger.Error(exception, "Stopped program because of exception");
				throw;
			}
			finally
			{
				// остановка логера 
				NLog.LogManager.Shutdown();
			}

			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			})
			.ConfigureLogging(logging =>
			{
				logging.AddDebug();
				logging.ClearProviders(); // создание провайдеров логирования
				logging.SetMinimumLevel(LogLevel.Trace); // устанавливаем минимальный уровень логирования
			}).UseNLog(); // добавляем библиотеку nlog
	}
}