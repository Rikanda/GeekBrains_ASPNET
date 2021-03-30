using MetricsAgent.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MetricsAgent.Responses.RamMetricsResponses;

namespace MetricsAgent.Controllers
{
	[Route("api/metrics/ram")]
	[ApiController]
	public class RamMetricsController : ControllerBase
	{
		private readonly ILogger<RamMetricsController> _logger;
		private readonly IRamMetricsRepository repository;

		public RamMetricsController(ILogger<RamMetricsController> logger, IRamMetricsRepository repository)
		{
			_logger = logger;
			_logger.LogDebug("Вызов конструктора");
			this.repository = repository;
		}

		[HttpGet("available")]
		public IActionResult GetMetrics()
		{
			_logger.LogDebug("Вызов метода");

			var metric = repository.GetLast();

			RamMetricDto response = null;

			if (metric != null)
			{
				response = new RamMetricDto { Time = metric.Time, Value = metric.Value, Id = metric.Id };
			}

			return Ok(response);
		}
	}
}
