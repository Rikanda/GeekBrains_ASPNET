using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Controllers
{
	[Route("api/metrics/hdd")]
	[ApiController]
	public class HddMetricsController : ControllerBase
	{
		private readonly ILogger<HddMetricsController> _logger;

		public HddMetricsController(ILogger<HddMetricsController> logger)
		{
			_logger = logger;
			_logger.LogDebug("Вызов конструктора");
		}

		[HttpGet("agent/left")]
		public IActionResult GetMetricsFromAgent()
		{
			_logger.LogDebug("Вызов метода");

			return Ok();
		}
	}
}
