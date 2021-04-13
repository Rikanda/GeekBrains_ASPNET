using MetricsAgent.DAL;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.ScheduledWorks
{
	/// <summary>
	/// Задача сбора Ram метрик
	/// </summary>
	public class RamMetricJob : IJob
	{
		// Инжектируем DI провайдер
		private readonly IServiceProvider _provider;
		private IRamMetricsRepository _repository;

		/// <summary>Имя категории счетчика</summary>
		private readonly string categoryName = "Memory";
		/// <summary>Имя счетчика</summary>
		private readonly string counterName = "Available MBytes";
		/// <summary>Счетчик</summary>
		private PerformanceCounter _counter;


		public RamMetricJob(IServiceProvider provider)
		{
			_provider = provider;
			_repository = _provider.GetService<IRamMetricsRepository>();

			_counter = new PerformanceCounter(categoryName, counterName);

		}

		public Task Execute(IJobExecutionContext context)
		{
			// Складываем характеристики всех экземпляров счетчиков
			int value = Convert.ToInt32(_counter.NextValue());

			// Время когда была собрана метрика
			var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

			// Запись метрики в репозиторий
			_repository.Create(new RamMetric { Time = time, Value = value });

			return Task.CompletedTask;
		}
	}
}