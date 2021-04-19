using System;
using NLog.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace MetricsManager
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// Disabling certificate validation can expose you to a man-in-the-middle attack
			// which may allow your encrypted message to be read by an attacker
			// https://stackoverflow.com/a/14907718/740639
			ServicePointManager.ServerCertificateValidationCallback =
				delegate (
					object s,
					X509Certificate certificate,
					X509Chain chain,
					SslPolicyErrors sslPolicyErrors
				)
				{
					return true;
				};

			var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
			try
			{
				logger.Info("---- [BEGIN]----");
				CreateHostBuilder(args).Build().Run();
			}
			// отлов всех исключений в рамках работы приложения
			catch (Exception exception)
			{
				//NLog: устанавливаем отлов исключений
				logger.Error(exception, "Stopped program because of exception");
			}
			finally
			{
				// остановка логера 
				NLog.LogManager.Shutdown();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>().UseUrls("http://localhost:5050");
			})
			.ConfigureLogging(logging =>
			{
				logging.AddDebug();
				logging.ClearProviders(); // создание провайдеров логирования
				logging.SetMinimumLevel(LogLevel.Trace); // устанавливаем минимальный уровень логирования
			})
			.UseNLog(); // добавляем библиотеку nlog

	}
}