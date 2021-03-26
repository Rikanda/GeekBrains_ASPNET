using Metrics.Tools;
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

		public CpuMetricsController(ILogger<CpuMetricsController> logger)
		{
			_logger = logger;
			_logger.LogInformation("CpuMetricsAgent created");


		}

		[HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAgent(
			[FromRoute] int agentId, 
			[FromRoute] TimeSpan fromTime, 
			[FromRoute] TimeSpan toTime)
		{
			_logger.LogInformation("Вызван метод CpuMetricsController, для получения данных от агента.");
			return Ok();
		}

		[HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentileFromAgent(
			[FromRoute] int agentId,
			[FromRoute] TimeSpan fromTime, 
			[FromRoute] TimeSpan toTime,
			[FromRoute] Percentile percentile)
		{
			return Ok();
		}

		[HttpGet("cluster/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAllCluster(
			[FromRoute] TimeSpan fromTime,
			[FromRoute] TimeSpan toTime)
		{
			return Ok();
		}

		[HttpGet("cluster/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentileFromAllCluster(
			[FromRoute] TimeSpan fromTime, 
			[FromRoute] TimeSpan toTime,
			[FromRoute] Percentile percentile)
		{
			return Ok();
		}
	}
}
