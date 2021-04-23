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
	/// Задача сбора Cpu метрик
	/// </summary>
	[DisallowConcurrentExecution]
	public class CpuMetricJob : IJob
	{
		private readonly IServiceProvider _provider;
		private IMetricsClient _client;
		private readonly ILogger _logger;
		private IAllCpuMetrics _allCpuMetrics;

		public CpuMetricJob(
			IServiceProvider provider,
			IMetricsClient client,
			ILogger<CpuMetricJob> logger)
		{
			_provider = provider;
			_client = client;
			_logger = logger;
			_allCpuMetrics = _provider.GetService<IAllCpuMetrics>();
		}

		public Task Execute(IJobExecutionContext context)
		{
			_logger.LogDebug("== CpuMetricJob START - " +
				$"Time {DateTimeOffset.UtcNow}");

			var request = new CpuMetricGetByIntervalRequestByClient()
			{
				AgentId = _allCpuMetrics.AgentId,
				FromTime = _allCpuMetrics.LastTime,
				ToTime = DateTimeOffset.UtcNow,
			};

			var response = _client.GetMetrics<CpuMetricDto>(request, ApiNames.Cpu);

			// Перекладываем данные из Response в модели метрик
			var metrics = new List<CpuMetric>();
			foreach (var metricDto in response.Metrics)
			{
				metrics.Add(new CpuMetric
				{
					Time = metricDto.Time,
					Value = metricDto.Value
				});
			}

			_allCpuMetrics.AddMetrics(metrics);

			_logger.LogDebug("!= CpuMetricJob END - " +
				$"Time {DateTimeOffset.UtcNow}");
			return Task.CompletedTask;
		}
	}
}