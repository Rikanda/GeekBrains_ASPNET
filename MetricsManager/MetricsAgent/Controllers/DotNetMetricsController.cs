using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Controllers
{
	[Route("api/metrics/dotnet")]
	[ApiController]
	public class DotNetMetricsController : ControllerBase
	{
		private readonly ILogger<DotNetMetricsController> _logger;

		public DotNetMetricsController(ILogger<DotNetMetricsController> logger)
		{
			_logger = logger;
			_logger.LogDebug("Вызов конструктора");
		}

		[HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAgent(
			[FromRoute] TimeSpan fromTime,
			[FromRoute] TimeSpan toTime)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(fromTime)} = {fromTime}" +
				$" {nameof(toTime)} = {toTime}");

			return Ok();
		}
	}
}
