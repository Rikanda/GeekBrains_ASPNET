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
				

				logger.Debug("---- [BEGIN]----");
				logger.Info(" Program class logging levels:");
				logger.Info($"Trace: {logger.IsTraceEnabled}");
				logger.Info($"Debug: {logger.IsDebugEnabled}");
				logger.Info($"Info: {logger.IsInfoEnabled}");
				logger.Info($"Warn: {logger.IsWarnEnabled}");
				logger.Info($"Error: {logger.IsErrorEnabled}");
				logger.Info($"Fatal: {logger.IsFatalEnabled}");

				//Samples
				logger.Info(" Program class test samples");
				logger.Trace("1 -- TRACE --");
				logger.Debug("2 -- DEBUG --");
				logger.Info(" 3 -- INFO  --");
				logger.Warn(" 4 -- WARN  --");
				logger.Error("5 -- ERROR --");
				logger.Fatal("6 -- FATAL --"); 
				
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