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

		public RamMetricsController(ILogger<RamMetricsController> logger)
		{
			_logger = logger;
			_logger.LogDebug("Вызов конструктора");
		}

		[HttpGet("available")]
		public IActionResult GetMetricsFromAgent()
		{
			_logger.LogDebug("Вызов метода");

			return Ok();
		}
	}
}
