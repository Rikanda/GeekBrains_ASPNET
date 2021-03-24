using Metrics.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Controllers
{
	[Route("api/metrics/cpu")]
	[ApiController]
	public class CpuMetricsController : ControllerBase
	{
		#region ---- !DELETE Временный отладочный метод для проверки сервера ----
		[HttpGet("read")]
		public IActionResult Read()
		{
			return Ok("Запуск MetricsAgent прошел успешно");
		}
		#endregion

		[HttpGet("from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAgent(
			[FromRoute] TimeSpan fromTime,
			[FromRoute] TimeSpan toTime)
		{
			return Ok();
		}

		[HttpGet("from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentileFromAgent(
			[FromRoute] TimeSpan fromTime,
			[FromRoute] TimeSpan toTime,
			[FromRoute] Percentile percentile)
		{
			return Ok();
		}

	}
}
