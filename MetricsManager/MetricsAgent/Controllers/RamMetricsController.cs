using MetricsAgent.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Controllers
{
	[Route("api/metrics/ram")]
	[ApiController]
	public class RamMetricsController : ControllerBase
	{
		private readonly ILogger<RamMetricsController> _logger;
		private IRamMetricsRepository repository;

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

			return Ok();
		}
	}
}
