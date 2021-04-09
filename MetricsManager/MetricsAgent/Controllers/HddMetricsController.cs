using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using MetricsAgent.Requests;
using System.Collections.Generic;

namespace MetricsAgent.Controllers
{
	/// <summary>
	/// Контроллер для обработки Hdd метрик
	/// </summary>
	[Route("api/metrics/hdd")]
	[ApiController]
	public class HddMetricsController : ControllerBase
	{
		private readonly ILogger<HddMetricsController> _logger;
		private readonly IHddMetricsRepository _repository;
		private readonly IMapper _mapper;

		public HddMetricsController(ILogger<HddMetricsController> logger, IHddMetricsRepository repository, IMapper mapper)
		{
			_logger = logger;
			_logger.LogDebug("Вызов конструктора");
			_repository = repository;
			_mapper = mapper;
		}

		/// <summary>
		/// Получение Hdd метрик за заданный промежуток времени
		/// </summary>
		/// <param name="request">Запрос на выдачу метрик с интервалом времени</param>
		/// <returns>Список метрик за заданный интервал времени</returns>
		[HttpGet("from/{request.fromTime}/to/{request.toTime}")]
		public IActionResult GetMetrics([FromRoute] HddMetricGetByIntervalRequest request)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.fromTime)} = {request.fromTime}" +
				$" {nameof(request.toTime)} = {request.toTime}");

			var metrics = _repository.GetByTimeInterval(request.fromTime, request.toTime);

			var response = new AllHddMetricsResponse()
			{
				Metrics = new List<HddMetricDto>()
			};

			foreach (var metric in metrics)
			{
				response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
			}

			return Ok(response);
		}

		/// <summary>
		/// Получение последней собранной Hdd метрики из базы данных
		/// </summary>
		/// <returns>Последняя собранная метрика из базы данных</returns>
		[HttpGet("left")]
		public IActionResult GetMetrics()
		{
			_logger.LogDebug("Вызов метода");

			var metric = _repository.GetLast();

			HddMetricDto response = null;

			response = _mapper.Map<HddMetricDto>(metric);

			return Ok(response);
		}
	}
}
