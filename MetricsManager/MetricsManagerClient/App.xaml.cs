using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Windows;

namespace MetricsManagerClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
		private readonly ServiceProvider _serviceProvider;
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
			services.AddSingleton<MainWindow>();

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
			//// Планировщики заданий
			//services.AddSingleton<IJobFactory, SingletonJobFactory>();
			//services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

			//// Задачи для разных метрик
			//services.AddSingleton<CpuMetricJob>();
			//services.AddSingleton<DotNetMetricJob>();
			//services.AddSingleton<HddMetricJob>();
			//services.AddSingleton<NetworkMetricJob>();
			//services.AddSingleton<RamMetricJob>();

			//// Периодичность запуска задач
			//services.AddSingleton(new JobSchedule(
			//	jobType: typeof(CpuMetricJob),
			//	cronExpression: CronExpression));
			//services.AddSingleton(new JobSchedule(
			//	jobType: typeof(DotNetMetricJob),
			//	cronExpression: CronExpression));
			//services.AddSingleton(new JobSchedule(
			//	jobType: typeof(HddMetricJob),
			//	cronExpression: CronExpression));
			//services.AddSingleton(new JobSchedule(
			//	jobType: typeof(NetworkMetricJob),
			//	cronExpression: CronExpression));
			//services.AddSingleton(new JobSchedule(
			//	jobType: typeof(RamMetricJob),
			//	cronExpression: CronExpression));

			//// Сервис для запуска задач с помощью Quarz
			//services.AddHostedService<QuartzHostedService>();

		}

		public static IHostBuilder CreateHostBuilder() =>
		Host.CreateDefaultBuilder()
		.ConfigureLogging(logging =>
		{
			logging.AddDebug();
			logging.ClearProviders(); // создание провайдеров логирования
			logging.SetMinimumLevel(LogLevel.Trace); // устанавливаем минимальный уровень логирования
		}).UseNLog(); // добавляем библиотеку nlog

	}
}
