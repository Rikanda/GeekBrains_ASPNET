using MetricsManager.DAL;
using MetricsManager.Responses;
using MetricsManager.Requests;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using MetricsManager.Client;
using Microsoft.Extensions.Logging;

namespace MetricsManager.ScheduledWorks
{
	/// <summary>
	/// Задача сбора Cpu метрик
	/// </summary>
	[DisallowConcurrentExecution]
	public class CpuMetricJob : IJob
	{
		// Инжектируем DI провайдер
		private readonly IServiceProvider _provider;
		private ICpuMetricsRepository _cpuRepository;
		private IAgentsRepository _agentsRepository;
		private IMapper _mapper;
		private IMetricsManagerClient _client;
		private readonly ILogger _logger;


		public CpuMetricJob(IServiceProvider provider, IMapper mapper, IMetricsManagerClient client, ILogger<CpuMetricJob> logger)
		{
			_provider = provider;
			_cpuRepository = _provider.GetService<ICpuMetricsRepository>();
			_agentsRepository = _provider.GetService<IAgentsRepository>();
			_mapper = mapper;
			_client = client;
			_logger = logger;


		}

		public Task Execute(IJobExecutionContext context)
		{
			_logger.LogDebug("== CpuMetricJob START - " +
				$"Time {DateTimeOffset.UtcNow}");
			//Получаем из репозитория агентов список всех агентов
			var allAgentsInfo = _agentsRepository.GetAllAgentsInfo();

			//Обрабатываем каждого агента в списке
			foreach (var agentInfo in allAgentsInfo)
			{
				//Временная метка, когда для текущего агента была снята последняя метрика
				var lastMetric = _cpuRepository.GetLast(agentInfo.AgentId);
				//Если метрик для этого агента не найдено, то берется минимально возможная дата
				var lastTime = lastMetric.Metrics.Count != 0 ? lastMetric.Metrics[0].Time : DateTimeOffset.FromUnixTimeSeconds(0);

				// Создаем запрос для получения от текущего агента метрик за период времени
				// от последней проверки до текущего момента
				var request = new CpuMetricGetByIntervalRequestByClient()
				{
					agentUri = agentInfo.AgentUri,
					fromTime = lastTime,
					toTime = DateTimeOffset.UtcNow,
				};

				// Делаем запрос к Агенту метрик и получаем список метрик
				var response = _client.GetCpuMetrics(request);

				if(response != null)
				{
					// Убираем из выборки первую метрику если она совпадает с последней сохраненной в базе
					// для исключения дублирования данных в базе
					if (response.Metrics[0].Time == lastTime)
					{
						response.Metrics.RemoveAt(0);
					}

					// Перекладываем данные из Response в модели метрик
					var recievedMetrics = new AllCpuMetrics() { Metrics = new List<CpuMetric>() };
					foreach (var metricDto in response.Metrics)
					{
						recievedMetrics.Metrics.Add(new CpuMetric
						{
							AgentId = agentInfo.AgentId,
							Time = metricDto.Time,
							Value = metricDto.Value
						});
					}
					_cpuRepository.Create(recievedMetrics);
				}

			}
			_logger.LogDebug("!= CpuMetricJob END - " +
				$"Time {DateTimeOffset.UtcNow}");
			return Task.CompletedTask;
		}
	}
}