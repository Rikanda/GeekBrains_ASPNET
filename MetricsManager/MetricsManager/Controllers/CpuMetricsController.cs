using AutoMapper;
using Metrics.Tools;
using MetricsManager.DAL;
using MetricsManager.Requests;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{
	[Route("api/metrics/cpu")]
	[ApiController]
	public class CpuMetricsController : ControllerBase
	{
		private readonly ILogger<CpuMetricsController> _logger;
		private readonly ICpuMetricsRepository _repository;
		private readonly IAgentsRepository _agentRepository;
		private readonly IMapper _mapper;

		public CpuMetricsController(
			ILogger<CpuMetricsController> logger, 
			ICpuMetricsRepository repository, 
			IAgentsRepository agentRepository, 
			IMapper mapper)
		{
			_logger = logger;
			_logger.LogDebug("Вызов конструктора");
			_repository = repository;
			_mapper = mapper;
			_agentRepository = agentRepository;
		}

		/// <summary>
		/// Запрос списка CPU метрик от определенного агента за промежуток времени
		/// </summary>
		/// <param name="request">Id агента и временной интервал</param>
		/// <returns>Список CPU метрик</returns>
		[HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAgent([FromRoute] CpuMetricGetByIntervalForAgentRequest request)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.agentId)} = {request.agentId}" +
				$" {nameof(request.fromTime)} = {request.fromTime}" +
				$" {nameof(request.toTime)} = {request.toTime}");

			var metrics = _repository.GetByTimeInterval(request.agentId, request.fromTime.ToLocalTime(), request.toTime.ToLocalTime());

			var response = new AllMetricsResponse<CpuMetricDto>();

			foreach (var metric in metrics.Metrics)
			{
				response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
			}

			return Ok(response);
		}

		/// <summary>
		/// Запрос перцентиля CPU метрик от определенного агента за промежуток времени
		/// </summary>
		/// <param name="request">Id агента и временной интервал</param>
		/// <param name="percentile">Перцентиль</param>
		/// <returns>Перцентиль CPU метрик</returns>
		[HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentileFromAgent(
			[FromRoute] CpuMetricGetByIntervalForAgentRequest request,
			[FromRoute] Percentile percentile)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.agentId)} = {request.agentId}" +
				$" {nameof(request.fromTime)} = {request.fromTime}" +
				$" {nameof(request.toTime)} = {request.toTime}" +
				$" {nameof(percentile)} = {percentile}");

			var metrics = _repository.GetByTimeIntervalPercentile(request.agentId, request.fromTime, request.toTime, percentile);

			var response = new AllMetricsResponse<CpuMetricDto>();

			foreach (var metric in metrics.Metrics)
			{
				response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
			}

			return Ok(response);
		}

		/// <summary>
		/// Запрос списка CPU метрик от всех агентов за промежуток времени
		/// </summary>
		/// <param name="request">Временной интервал</param>
		/// <returns>Список CPU метрик</returns>
		[HttpGet("cluster/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAllCluster([FromRoute] CpuMetricGetByIntervalForClusterRequest request)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.fromTime)} = {request.fromTime}" +
				$" {nameof(request.toTime)} = {request.toTime}");

			var agents = _agentRepository.GetAllAgentsInfo();

			var response = new AllMetricsResponse<CpuMetricDto>();

			foreach (var agent in agents.Agents)
			{
				var currentAgentMetrics = _repository.GetByTimeInterval(agent.AgentId, request.fromTime, request.toTime);

				foreach (var metric in currentAgentMetrics.Metrics)
				{
					response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
				}
			}

			return Ok(response);
		}

		/// <summary>
		/// Запрос перцентиля CPU метрик для каждого агента за промежуток времени
		/// </summary>
		/// <param name="request">Временной интервал</param>
		/// <param name="percentile">Перцентиль</param>
		/// <returns>Перцентиль CPU метрик</returns>
		[HttpGet("cluster/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentileFromAllCluster(
			[FromRoute] CpuMetricGetByIntervalForClusterRequest request,
			[FromRoute] Percentile percentile)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.fromTime)} = {request.fromTime}" +
				$" {nameof(request.toTime)} = {request.toTime}" +
				$" {nameof(percentile)} = {percentile}");

			var agents = _agentRepository.GetAllAgentsInfo();

			var response = new AllMetricsResponse<CpuMetricDto>();

			foreach (var agent in agents.Agents)
			{
				var currentAgentMetrics = _repository.GetByTimeIntervalPercentile(agent.AgentId, request.fromTime, request.toTime, percentile);

				foreach (var metric in currentAgentMetrics.Metrics)
				{
					response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
				}

			}

			return Ok(response);
		}
	}
}
