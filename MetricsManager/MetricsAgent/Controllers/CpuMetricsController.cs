using MetricsAgent.DAL;
using MetricsAgent.Models;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using Metrics.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Controllers
{
	[Route("api/metrics/cpu")]
	[ApiController]
	public class CpuMetricsController : ControllerBase
	{
		private readonly ILogger<CpuMetricsController> _logger;
		private ICpuMetricsRepository repository;

		public CpuMetricsController(ILogger<CpuMetricsController> logger, ICpuMetricsRepository repository)
		{
			_logger = logger;
			_logger.LogDebug("CpuMetricsController");
			this.repository = repository;
		}

		[HttpPost("create")]
		public IActionResult Create([FromBody] CpuMetricCreateRequest request)
		{
			repository.Create(new CpuMetric
			{
				Time = TimeSpan.FromSeconds(request.Time),
				Value = request.Value
			});

			return Ok();
		}

		[HttpGet("all")]
		public IActionResult GetAll()
		{
			var metrics = repository.GetAll();

			var response = new AllCpuMetricsResponse()
			{
				Metrics = new List<CpuMetricDto>()
			};

			foreach (var metric in metrics)
			{
				response.Metrics.Add(new CpuMetricDto { Time = metric.Time, Value = metric.Value, Id = metric.Id });
			}

			return Ok(response);
		}


		[HttpGet("from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAgent(
			[FromRoute] TimeSpan fromTime,
			[FromRoute] TimeSpan toTime)
		{
			_logger.LogInformation("Вызван GET метод CpuMetricsFromAgent");
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
