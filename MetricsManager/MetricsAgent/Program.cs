using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MetricsAgent
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
