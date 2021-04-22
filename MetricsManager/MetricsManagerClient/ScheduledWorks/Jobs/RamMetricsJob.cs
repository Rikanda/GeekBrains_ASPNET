using MetricsManagerClient.Client;
using MetricsManagerClient.Models;
using MetricsManagerClient.Models.Metrics;
using MetricsManagerClient.Requests;
using MetricsManagerClient.Responses.FromManager;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MetricsManagerClient.ScheduledWorks.Jobs
{
	/// <summary>
	/// Задача сбора Ram метрик
	/// </summary>
	[DisallowConcurrentExecution]
	public class RamMetricJob : IJob
	{
		private readonly IServiceProvider _provider;
		private IMetricsClient _client;
		private readonly ILogger _logger;
		private IAllRamMetrics _allRamMetrics;

		public RamMetricJob(
			IServiceProvider provider,
			IMetricsClient client,
			ILogger<RamMetricJob> logger)
		{
			_provider = provider;
			_client = client;
			_logger = logger;
			_allRamMetrics = _provider.GetService<IAllRamMetrics>();
		}

		public Task Execute(IJobExecutionContext context)
		{
			_logger.LogDebug("== RamMetricJob START - " +
				$"Time {DateTimeOffset.UtcNow}");

			var request = new RamMetricGetByIntervalRequestByClient()
			{
				AgentId = _allRamMetrics.AgentId,
				FromTime = _allRamMetrics.LastTime,
				ToTime = DateTimeOffset.UtcNow,
			};

			var response = _client.GetMetrics<RamMetricDto>(request, ApiNames.Ram);

			// Перекладываем данные из Response в модели метрик
			var metrics = new List<RamMetric>();
			foreach (var metricDto in response.Metrics)
			{
				metrics.Add(new RamMetric
				{
					Time = metricDto.Time,
					Value = metricDto.Value
				});
			}

			_allRamMetrics.AddMetrics(metrics);

			_logger.LogDebug("!= RamMetricJob END - " +
				$"Time {DateTimeOffset.UtcNow}");
			return Task.CompletedTask;
		}
	}
}