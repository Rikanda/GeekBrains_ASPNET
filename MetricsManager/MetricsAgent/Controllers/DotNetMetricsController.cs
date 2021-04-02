using MetricsAgent.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MetricsAgent.Responses.DotNetMetricsResponses;

namespace MetricsAgent.Controllers
{
	[Route("api/metrics/dotnet")]
	[ApiController]
	public class DotNetMetricsController : ControllerBase
	{
		private readonly ILogger<DotNetMetricsController> _logger;
		private readonly IDotNetMetricsRepository repository;

		public DotNetMetricsController(ILogger<DotNetMetricsController> logger, IDotNetMetricsRepository repository)
		{
			_logger = logger;
			_logger.LogDebug("Вызов конструктора");
			this.repository = repository;
		}

		[HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetrics(
			[FromRoute] DateTimeOffset fromTime,
			[FromRoute] DateTimeOffset toTime)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(fromTime)} = {fromTime}" +
				$" {nameof(toTime)} = {toTime}");

			var metrics = repository.GetByTimeInterval(fromTime, toTime);

			var response = new AllDotNetMetricsResponse()
			{
				Metrics = new List<DotNetMetricDto>()
			};

			foreach (var metric in metrics)
			{
				response.Metrics.Add(new DotNetMetricDto { Time = metric.Time, Value = metric.Value, Id = metric.Id });
			}

			return Ok(response);
		}
	}
}
