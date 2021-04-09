using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Metrics.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using AutoMapper;

namespace MetricsAgent.Controllers
{
	/// <summary>
	/// Контроллер для обработки Cpu метрик
	/// </summary>
	[Route("api/metrics/cpu")]
	[ApiController]
	public class CpuMetricsController : ControllerBase
	{
		private readonly ILogger<CpuMetricsController> _logger;
		private readonly ICpuMetricsRepository _repository;
		private readonly IMapper _mapper;

		public CpuMetricsController(ILogger<CpuMetricsController> logger, ICpuMetricsRepository repository, IMapper mapper)
		{
			_logger = logger;
			_logger.LogDebug("Вызов конструктора");
			_repository = repository;
			_mapper = mapper;
		}

		/// <summary>
		/// Получение CPU метрик за заданный промежуток времени
		/// </summary>
		/// <param name="fromTime">Начало временного промежутка</param>
		/// <param name="toTime">Конец временного промежутка</param>
		/// <returns>Список метрик за заданный интервал времени</returns>
		[HttpGet("from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetrics(
			[FromRoute] DateTimeOffset fromTime,
			[FromRoute] DateTimeOffset toTime)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(fromTime)} = {fromTime}" +
				$" {nameof(toTime)} = {toTime}");

			var metrics = _repository.GetByTimeInterval(fromTime, toTime);

			var response = new AllCpuMetricsResponse()
			{
				Metrics = new List<CpuMetricDto>()
			};

			foreach (var metric in metrics)
			{
				response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
			}

			return Ok(response);
		}

		[HttpGet("from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentile(
			[FromRoute] DateTimeOffset fromTime,
			[FromRoute] DateTimeOffset toTime,
			[FromRoute] Percentile percentile)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(fromTime)} = {fromTime}" +
				$" {nameof(toTime)} = {toTime}" +
				$" {nameof(percentile)} = {percentile}");

			return Ok();
		}

	}
}
