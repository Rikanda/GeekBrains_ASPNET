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
				CreateHostBuilder(args).Build().Run();
			}
			// ����� ���� ���������� � ������ ������ ����������
			catch (Exception exception)
			{
				//NLog: ������������� ����� ����������
				logger.Error(exception, "Stopped program because of exception");
			}
			finally
			{
				// ��������� ������ 
				NLog.LogManager.Shutdown();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			})
			.ConfigureLogging(logging =>
			{
				logging.AddDebug();
				logging.ClearProviders(); // �������� ����������� �����������
				logging.SetMinimumLevel(LogLevel.Trace); // ������������� ����������� ������� �����������
			}).UseNLog(); // ��������� ���������� nlog
	}
}