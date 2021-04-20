using MetricsManagerClient.Client;
using MetricsManagerClient.Models;
using MetricsManagerClient.Requests;
using MetricsManagerClient.Responses.FromManager;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace MetricsManagerClient.ScheduledWorks.Jobs
{
	/// <summary>
	/// Задача сбора Cpu метрик
	/// </summary>
	[DisallowConcurrentExecution]
	public class CpuMetricJob : IJob
	{
		// Инжектируем DI провайдер
		private readonly IServiceProvider _provider;
		private IMetricsClient _client;
		private readonly ILogger _logger;
		private AllCpuMetrics _allCpuMetrics;


		public CpuMetricJob(
			IServiceProvider provider,
			IMetricsClient client,
			ILogger<CpuMetricJob> logger,
			AllCpuMetrics allCpuMetrics)
		{
			_provider = provider;
			_client = client;
			_logger = logger;
			_allCpuMetrics = allCpuMetrics;


		}

		public Task Execute(IJobExecutionContext context)
		{
			_logger.LogDebug("== CpuMetricJob START - " +
				$"Time {DateTimeOffset.UtcNow}");

			var metrics = new AllMetricsResponse<CpuMetricDto>();

			var request = new CpuMetricGetByIntervalRequestByClient()
			{
				AgentId = _allCpuMetrics.AgentId,
					FromTime = _allCpuMetrics.LastTime,
					ToTime = DateTimeOffset.UtcNow,
			};
			metrics = _client.GetMetrics<CpuMetricDto>(request, ApiNames.Cpu);

			foreach(var metric in metrics.Metrics)
			{
				_allCpuMetrics.AddMetric(metric.Value, metric.Time);
			}

			_logger.LogDebug("!= CpuMetricJob END - " +
				$"Time {DateTimeOffset.UtcNow}");
			return Task.CompletedTask;
		}
	}
}