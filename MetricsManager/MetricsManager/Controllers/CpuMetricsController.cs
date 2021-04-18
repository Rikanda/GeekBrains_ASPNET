using AutoMapper;
using Metrics.Tools;
using MetricsManager.DAL;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Repositories;
using MetricsManager.Requests;
using MetricsManager.Responses;
using MetricsManager.Responses.FromManager;
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

		[HttpGet("agent/{request.agentId}/from/{request.fromTime}/to/{request.toTime}")]
		public IActionResult GetMetricsFromAgent([FromRoute] CpuMetricGetByIntervalForAgentRequest request)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.AgentId)} = {request.AgentId}" +
				$" {nameof(request.FromTime)} = {request.FromTime}" +
				$" {nameof(request.ToTime)} = {request.ToTime}");

			var metrics = _repository.GetByTimeInterval(request.AgentId, request.FromTime, request.ToTime);

			var response = new AllMetricsResponse<CpuMetricDto>();

			foreach (var metric in metrics.Metrics)
			{
				response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
			}

			return Ok(response);
		}

		[HttpGet("agent/{request.agentId}/from/{request.fromTime}/to/{request.toTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentileFromAgent(
			[FromRoute] CpuMetricGetByIntervalForAgentRequest request,
			[FromRoute] Percentile percentile)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.AgentId)} = {request.AgentId}" +
				$" {nameof(request.FromTime)} = {request.FromTime}" +
				$" {nameof(request.ToTime)} = {request.ToTime}" +
				$" {nameof(percentile)} = {percentile}");

			var metrics = _repository.GetByTimeIntervalPercentile(request.AgentId, request.FromTime, request.ToTime, percentile);

			var response = new AllMetricsResponse<CpuMetricDto>();

			foreach (var metric in metrics.Metrics)
			{
				response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
			}

			return Ok(response);
		}

		[HttpGet("cluster/from/{request.fromTime}/to/{request.toTime}")]
		public IActionResult GetMetricsFromAllCluster([FromRoute] CpuMetricGetByIntervalForClusterRequest request)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.FromTime)} = {request.FromTime}" +
				$" {nameof(request.ToTime)} = {request.ToTime}");

			var agents = _agentRepository.GetAllAgentsInfo();

			var response = new AllMetricsResponse<CpuMetricDto>();

			foreach (var agent in agents.Agents)
			{
				var currentAgentMetrics = _repository.GetByTimeInterval(agent.AgentId, request.FromTime, request.ToTime);

				foreach (var metric in currentAgentMetrics.Metrics)
				{
					response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
				}
			}

			return Ok(response);
		}

		[HttpGet("cluster/from/{request.fromTime}/to/{request.toTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentileFromAllCluster(
			[FromRoute] CpuMetricGetByIntervalForClusterRequest request,
			[FromRoute] Percentile percentile)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.FromTime)} = {request.FromTime}" +
				$" {nameof(request.ToTime)} = {request.ToTime}" +
				$" {nameof(percentile)} = {percentile}");

			var agents = _agentRepository.GetAllAgentsInfo();

			var response = new AllMetricsResponse<CpuMetricDto>();

			foreach (var agent in agents.Agents)
			{
				var currentAgentMetrics = _repository.GetByTimeIntervalPercentile(agent.AgentId, request.FromTime, request.ToTime, percentile);

				foreach (var metric in currentAgentMetrics.Metrics)
				{
					response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
				}

			}

			return Ok(response);
		}
	}
}
