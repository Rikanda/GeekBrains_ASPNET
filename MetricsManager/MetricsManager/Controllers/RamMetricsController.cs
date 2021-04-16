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
	[Route("api/metrics/ram")]
	[ApiController]
	public class RamMetricsController : ControllerBase
	{
		private readonly ILogger<RamMetricsController> _logger;
		private readonly IRamMetricsRepository _repository;
		private readonly IAgentsRepository _agentRepository;
		private readonly IMapper _mapper;

		public RamMetricsController(
			ILogger<RamMetricsController> logger,
			IRamMetricsRepository repository,
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
		/// Запрос списка RAM метрик от определенного агента за промежуток времени
		/// </summary>
		/// <param name="request">Id агента и временной интервал</param>
		/// <returns>Список RAM метрик</returns>
		[HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAgent([FromRoute] RamMetricGetByIntervalForAgentRequest request)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.agentId)} = {request.agentId}" +
				$" {nameof(request.fromTime)} = {request.fromTime}" +
				$" {nameof(request.toTime)} = {request.toTime}");

			var metrics = _repository.GetByTimeInterval(request.agentId, request.fromTime, request.toTime);

			var response = new AllMetricsResponse<RamMetricDto>();

			foreach (var metric in metrics.Metrics)
			{
				response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
			}

			return Ok(response);
		}

		/// <summary>
		/// Запрос перцентиля RAM метрик от определенного агента за промежуток времени
		/// </summary>
		/// <param name="request">Id агента и временной интервал</param>
		/// <param name="percentile">Перцентиль</param>
		/// <returns>Перцентиль RAM метрик</returns>
		[HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentileFromAgent(
			[FromRoute] RamMetricGetByIntervalForAgentRequest request,
			[FromRoute] Percentile percentile)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.agentId)} = {request.agentId}" +
				$" {nameof(request.fromTime)} = {request.fromTime}" +
				$" {nameof(request.toTime)} = {request.toTime}" +
				$" {nameof(percentile)} = {percentile}");

			var metrics = _repository.GetByTimeIntervalPercentile(request.agentId, request.fromTime, request.toTime, percentile);

			var response = new AllMetricsResponse<RamMetricDto>();

			foreach (var metric in metrics.Metrics)
			{
				response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
			}

			return Ok(response);
		}

		/// <summary>
		/// Запрос списка RAM метрик от всех агентов за промежуток времени
		/// </summary>
		/// <param name="request">Временной интервал</param>
		/// <returns>Список RAM метрик</returns>
		[HttpGet("cluster/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAllCluster([FromRoute] RamMetricGetByIntervalForClusterRequest request)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.fromTime)} = {request.fromTime}" +
				$" {nameof(request.toTime)} = {request.toTime}");

			var agents = _agentRepository.GetAllAgentsInfo();

			var response = new AllMetricsResponse<RamMetricDto>();

			foreach (var agent in agents.Agents)
			{
				var currentAgentMetrics = _repository.GetByTimeInterval(agent.AgentId, request.fromTime, request.toTime);

				foreach (var metric in currentAgentMetrics.Metrics)
				{
					response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
				}
			}

			return Ok(response);
		}

		/// <summary>
		/// Запрос перцентиля RAM метрик для каждого агента за промежуток времени
		/// </summary>
		/// <param name="request">Временной интервал</param>
		/// <param name="percentile">Перцентиль</param>
		/// <returns>Перцентиль RAM метрик</returns>
		[HttpGet("cluster/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentileFromAllCluster(
			[FromRoute] RamMetricGetByIntervalForClusterRequest request,
			[FromRoute] Percentile percentile)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.fromTime)} = {request.fromTime}" +
				$" {nameof(request.toTime)} = {request.toTime}" +
				$" {nameof(percentile)} = {percentile}");

			var agents = _agentRepository.GetAllAgentsInfo();

			var response = new AllMetricsResponse<RamMetricDto>();

			foreach (var agent in agents.Agents)
			{
				var currentAgentMetrics = _repository.GetByTimeIntervalPercentile(agent.AgentId, request.fromTime, request.toTime, percentile);

				foreach (var metric in currentAgentMetrics.Metrics)
				{
					response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
				}

			}

			return Ok(response);
		}
	}
}
