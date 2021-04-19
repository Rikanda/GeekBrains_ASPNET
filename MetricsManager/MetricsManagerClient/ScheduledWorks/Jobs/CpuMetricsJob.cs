using MetricsManagerClient.Client;
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
		private MainWindow _mainWindow;


		public CpuMetricJob(
			IServiceProvider provider,
			IMetricsClient client,
			ILogger<CpuMetricJob> logger,
			MainWindow mainWindow)
		{
			_provider = provider;
			_client = client;
			_logger = logger;
			_mainWindow = mainWindow;


		}

		public Task Execute(IJobExecutionContext context)
		{
			_logger.LogDebug("== CpuMetricJob START - " +
				$"Time {DateTimeOffset.UtcNow}");
			//Получаем из репозитория агентов список всех агентов
			var allAgentsInfo = _client.GetAllAgentsInfo();

			////Обрабатываем каждого агента в списке
			//foreach (var agentInfo in allAgentsInfo.Agents)
			//{

			//!DEBUG берем только первого агента в списке
			// Создаем запрос для получения от текущего агента метрик за период времени
			// за 50 секунд от текущего момента

			var metrics = new AllMetricsResponse<CpuMetricDto>();

			if(allAgentsInfo.Agents.Count!=0)
			{
				//var timeNow = DateTimeOffset.Parse("2021-04-15T14:14:50Z");
				var timeNow = DateTimeOffset.UtcNow;
				var request = new CpuMetricGetByIntervalRequestByClient()
				{
					AgentId = allAgentsInfo.Agents[0].AgentId,
					ToTime = timeNow,
					FromTime = timeNow - TimeSpan.FromSeconds(54),
				};
				metrics = _client.GetMetrics<CpuMetricDto>(request, ApiNames.Cpu);
			}

			var values = _mainWindow.CpuChart.ColumnServiesValues[0].Values;

			values.Clear();
			for (int i = 0; i < metrics.Metrics.Count; i++)
			{
			    values.Add((double)metrics.Metrics[i].Value);
 			}

			//CpuChart.ColumnServiesValues[0].Values.Add(new Random().NextDouble() * 100);
			//CpuChart.ColumnServiesValues[0].Values.RemoveAt(0);


			//	// Делаем запрос к Агенту метрик и получаем список метрик
			//	var response = _client.GetMetrics<CpuMetricDto>(request, ApiNames.Cpu);

			//	if (response != null)
			//	{
			//		//!DEBUG Передаем метрики в куда-нибудь в вывод на экран
			//	}

			//}
			_logger.LogDebug("!= CpuMetricJob END - " +
				$"Time {DateTimeOffset.UtcNow}");
			return Task.CompletedTask;
		}
	}
}