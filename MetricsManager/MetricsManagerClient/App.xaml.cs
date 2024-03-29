﻿using MetricsManagerClient.Client;
using MetricsManagerClient.Models;
using MetricsManagerClient.ScheduledWorks.Jobs;
using MetricsManagerClient.ScheduledWorks.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Windows;

namespace MetricsManagerClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
		/// <summary>
		/// Периодичность запуска задач по сбору метрик
		/// </summary>
		private const string CronExpression = "0/5 * * * * ?";

		private IHost _host;
		private NLog.Logger _logger;

		public App()
		{
			var serviceCollection = new ServiceCollection();
			_logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

			_host = new HostBuilder()
				.ConfigureServices((hostContext, services) =>
				{
					ConfigureServices(services);
				})
			.ConfigureLogging(logging =>
			{
				logging.AddDebug();
				logging.ClearProviders(); // создание провайдеров логирования
				logging.SetMinimumLevel(LogLevel.Trace); // устанавливаем минимальный уровень логирования
			})
			.UseNLog()  // добавляем библиотеку nlog
			.Build();
		}

		/// <summary>
		/// Конфигурация сервисов
		/// </summary>
		/// <param name="services">Контейнер для сервисов</param>
		private void ConfigureServices(IServiceCollection services)
		{
			// Главное окно
			services.AddSingleton<MainWindow>();

			// Модели собирающие метрики
			services.AddSingleton<IAllCpuMetrics, AllCpuMetrics>();
			services.AddSingleton<IAllRamMetrics, AllRamMetrics>();

			// Клиент для запросов к Менеджеру метрик
			services.AddHttpClient<IMetricsClient, MetricsClient>();


			JobsSheduleSettings(services);
		}

		private async void OnStartup(object sender, StartupEventArgs e)
		{

			using (var serviceScope = _host.Services.CreateScope())
			{
				var services = serviceScope.ServiceProvider;
				try
				{
					await _host.StartAsync();

					_logger.Trace("----[BEGIN]---- (trace)");
					_logger.Debug("----[BEGIN]---- (debug)");
					_logger.Info("----[BEGIN]---- (info)");

					var mainWindow = services.GetRequiredService<MainWindow>();
					mainWindow.Show();
				}
				catch (Exception exception)
				{
					_logger.Error(exception, "Stopped program because of exception");
				}
			}

		}

		protected override async void OnExit(ExitEventArgs e)
		{
			using (_host)
			{
				_logger.Info("---- [END] ---- (info)");
				_logger.Debug("---- [END] ---- (debug)");
				_logger.Trace("---- [END] ---- (trace)");
				// остановка логера 
				NLog.LogManager.Shutdown();

				await _host.StopAsync(TimeSpan.FromSeconds(5));
			}

			base.OnExit(e);
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
			services.AddSingleton<RamMetricJob>();

			// Периодичность запуска задач
			services.AddSingleton(new JobSchedule(
				jobType: typeof(CpuMetricJob),
				cronExpression: CronExpression));
			services.AddSingleton(new JobSchedule(
				jobType: typeof(RamMetricJob),
				cronExpression: CronExpression));

			// Сервис для запуска задач с помощью Quarz
			services.AddHostedService<QuartzHostedService>();

		}

	}
}
