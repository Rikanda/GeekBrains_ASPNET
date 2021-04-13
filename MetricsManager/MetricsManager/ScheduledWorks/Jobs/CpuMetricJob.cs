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

namespace MetricsManager.ScheduledWorks
{
	/// <summary>
	/// Задача сбора Cpu метрик
	/// </summary>
	public class CpuMetricJob : IJob
	{
		// Инжектируем DI провайдер
		private readonly IServiceProvider _provider;
		private ICpuMetricsRepository _cpuRepository;
		private IAgentsRepository _agentsRepository;
		private IMapper _mapper;


		public CpuMetricJob(IServiceProvider provider, IMapper mapper)
		{
			_provider = provider;
			_cpuRepository = _provider.GetService<ICpuMetricsRepository>();
			_agentsRepository = _provider.GetService<IAgentsRepository>();
			_mapper = mapper;


		}

		public Task Execute(IJobExecutionContext context)
		{
			//Получаем из репозитория агентов список всех агентов
			var allAgentsInfo = _agentsRepository.GetAllAgentsInfo();

			//Обрабатываем каждого агента в списке
			foreach (var agentInfo in allAgentsInfo)
			{
				//Временная метка, когда для текущего агента была снята последняя метрика
				var lastTime = _cpuRepository.GetLast(agentInfo.AgentId).Time;

				//Создаем запрос для получения от текущего агента метрик за период времени
				//от последней проверки до текущего момента
				var request = new CpuMetricGetByIntervalRequest()
				{
					agentId = agentInfo.AgentId,
					fromTime = lastTime,
					toTime = DateTimeOffset.UtcNow,
				};

				//!DEBUG здесь должен быть запрос к соответствующему агенту и получение от него списка метрик.
				//Типа так:
				//var response = _client.GetCpuMetricsByIntervalFromAgent
				//И в ответ должны получить, что-то вроде
				var response = new AllCpuMetricsResponse()
				{
					Metrics = new List<CpuMetricDto>()
					{
						new CpuMetricDto()
						{
							AgentId = agentInfo.AgentId,
							Time = lastTime,
							Value = new Random().Next(100),
						},
						new CpuMetricDto()
						{
							AgentId = agentInfo.AgentId,
							Time = DateTimeOffset.UtcNow,
							Value = new Random().Next(100),
						}
					}
				};
				//Теперь вот это нужно записать в базу данных менеджера


				foreach(var metricDto in response.Metrics)
				{
					_cpuRepository.Create(new CpuMetric 
					{ 
						AgentId = metricDto.AgentId, 
						Time = metricDto.Time, 
						Value = metricDto.Value });
				}

			}
			return Task.CompletedTask;
		}
	}
}