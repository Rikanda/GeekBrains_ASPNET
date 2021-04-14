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
	public class DotNetMetricJob : IJob
	{
		// Инжектируем DI провайдер
		private readonly IServiceProvider _provider;
		private IDotNetMetricsRepository _repository;
		private IAgentsRepository _agentsRepository;
		private IMapper _mapper;
		private IMetricsManagerClient _client;
		private readonly ILogger _logger;


		public DotNetMetricJob(IServiceProvider provider, IMapper mapper, IMetricsManagerClient client, ILogger<DotNetMetricJob> logger)
		{
			_provider = provider;
			_repository = _provider.GetService<IDotNetMetricsRepository>();
			_agentsRepository = _provider.GetService<IAgentsRepository>();
			_mapper = mapper;
			_client = client;
			_logger = logger;


		}

		public Task Execute(IJobExecutionContext context)
		{
			_logger.LogDebug("== DotNetMetricJob START - " +
				$"Time {DateTimeOffset.UtcNow}");
			//Получаем из репозитория агентов список всех агентов
			var allAgentsInfo = _agentsRepository.GetAllAgentsInfo();

			//Обрабатываем каждого агента в списке
			foreach (var agentInfo in allAgentsInfo.Agents)
			{
				//Временная метка, когда для текущего агента была снята последняя метрика
				var lastMetric = _repository.GetLast(agentInfo.AgentId);
				//Если метрик для этого агента не найдено, то берется минимально возможная дата
				var lastTime = lastMetric.Metrics.Count != 0 ? lastMetric.Metrics[0].Time : DateTimeOffset.FromUnixTimeSeconds(0);

				// Создаем запрос для получения от текущего агента метрик за период времени
				// от последней проверки до текущего момента
				var request = new DotNetMetricGetByIntervalRequestByClient()
				{
					agentUri = agentInfo.AgentUri,
					fromTime = lastTime,
					toTime = DateTimeOffset.UtcNow,
				};

				// Делаем запрос к Агенту метрик и получаем список метрик
				var response = _client.GetMetrics<DotNetMetricFromAgentDto>(request, ApiNames.DotNet);

				if (response != null)
				{
					// Убираем из выборки первую метрику если она совпадает с последней сохраненной в базе
					// для исключения дублирования данных в базе
					if (response.Metrics[0].Time == lastTime)
					{
						response.Metrics.RemoveAt(0);
					}

					// Перекладываем данные из Response в модели метрик
					var recievedMetrics = new AllMetrics<DotNetMetric>();
					foreach (var metricDto in response.Metrics)
					{
						recievedMetrics.Metrics.Add(new DotNetMetric
						{
							AgentId = agentInfo.AgentId,
							Time = metricDto.Time,
							Value = metricDto.Value
						});
					}
					_repository.Create(recievedMetrics);
				}

			}
			_logger.LogDebug("!= DotNetMetricJob END - " +
				$"Time {DateTimeOffset.UtcNow}");
			return Task.CompletedTask;
		}
	}
}