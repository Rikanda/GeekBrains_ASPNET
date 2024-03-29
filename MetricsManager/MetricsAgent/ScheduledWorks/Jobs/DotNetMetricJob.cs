﻿using MetricsAgent.DAL;
using MetricsAgent.DAL.Models;
using MetricsAgent.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.ScheduledWorks.Jobs
{
	/// <summary>
	/// Задача сбора DotNet метрик
	/// </summary>
	[DisallowConcurrentExecution]
	public class DotNetMetricJob : IJob
	{
		// Инжектируем DI провайдер
		private readonly IServiceProvider _provider;
		private IDotNetMetricsRepository _repository;

		/// <summary>Имя категории счетчика</summary>
		private readonly string categoryName = ".NET CLR Memory";
		/// <summary>Имя счетчика</summary>
		private readonly string counterName = "# Bytes in all Heaps";
		/// <summary>Счетчик</summary>
		private PerformanceCounter _counter;


		public DotNetMetricJob(IServiceProvider provider)
		{
			_provider = provider;
			_repository = _provider.GetService<IDotNetMetricsRepository>();

			_counter = new PerformanceCounter(categoryName, counterName, "_Global_");

		}

		public Task Execute(IJobExecutionContext context)
		{
			// Складываем характеристики всех экземпляров счетчиков
			int value = Convert.ToInt32(_counter.NextValue());

			// Время когда была собрана метрика
			var time = DateTimeOffset.UtcNow;

			// Запись метрики в репозиторий
			_repository.Create(new DotNetMetric { Time = time, Value = value });

			return Task.CompletedTask;
		}
	}
}