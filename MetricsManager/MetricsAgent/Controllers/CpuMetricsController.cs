using MetricsAgent.Enums;
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
		//!DELETE Отладочный метод для проверки сервера
		[HttpGet("read")]
		public IActionResult Read()
		{
			return Ok("Запуск прошел MetricsAgent успешно");
		}

		[HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAgent(
			[FromRoute] int agentId,
			[FromRoute] TimeSpan fromTime,
			[FromRoute] TimeSpan toTime)
		{
			return Ok();
		}

		[HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentileFromAgent(
			[FromRoute] int agentId,
			[FromRoute] TimeSpan fromTime,
			[FromRoute] TimeSpan toTime)
		{
			return Ok();
		}

	}
}
