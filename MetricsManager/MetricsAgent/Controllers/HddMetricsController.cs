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
	[Route("api/metrics/hdd")]
	[ApiController]
	public class HddMetricsController : ControllerBase
	{
		private readonly ILogger<HddMetricsController> _logger;
		private IHddMetricsRepository repository;

		public HddMetricsController(ILogger<HddMetricsController> logger, IHddMetricsRepository repository)
		{
			_logger = logger;
			_logger.LogDebug("Вызов конструктора");
			this.repository = repository;
		}

		[HttpGet("agent/left")]
		public IActionResult GetMetrics()
		{
			_logger.LogDebug("Вызов метода");

			return Ok();
		}
	}
}
