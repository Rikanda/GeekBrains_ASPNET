using MetricsAgent.DAL;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.ScheduledWorks
{
	/// <summary>
	/// Задача сбора Cpu метрик
	/// </summary>
	public class CpuMetricJob : IJob
	{
		// Инжектируем DI провайдер
		private readonly IServiceProvider _provider;
		private ICpuMetricsRepository _repository;

		/// <summary>Имя категории счетчика</summary>
		private readonly string categoryName = "Processor";
		/// <summary>Имя счетчика</summary>
		private readonly string counterName = "% Processor Time";
		/// <summary>Счетчик</summary>
		private PerformanceCounter _counter;


		public CpuMetricJob(IServiceProvider provider)
		{
			_provider = provider;
			_repository = _provider.GetService<ICpuMetricsRepository>();

			_counter = new PerformanceCounter(categoryName, counterName, "_Total");

		}

		public Task Execute(IJobExecutionContext context)
		{
			// Складываем характеристики всех экземпляров счетчиков
			int value = Convert.ToInt32(_counter.NextValue());

			// Время когда была собрана метрика
			var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

			// Запись метрики в репозиторий
			_repository.Create(new CpuMetric { Time = time, Value = value });

			return Task.CompletedTask;
		}
	}
}