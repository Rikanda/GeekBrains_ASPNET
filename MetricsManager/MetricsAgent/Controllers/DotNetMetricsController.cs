using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using AutoMapper;

namespace MetricsAgent.Controllers
{
	/// <summary>
	/// Контроллер для обработки DotNet метрик
	/// </summary>
	[Route("api/metrics/dotnet")]
	[ApiController]
	public class DotNetMetricsController : ControllerBase
	{
		private readonly ILogger<DotNetMetricsController> _logger;
		private readonly IDotNetMetricsRepository _repository;
		private readonly IMapper _mapper;

		public DotNetMetricsController(ILogger<DotNetMetricsController> logger, IDotNetMetricsRepository repository, IMapper mapper)
		{
			_logger = logger;
			_logger.LogDebug("Вызов конструктора");
			_repository = repository;
			_mapper = mapper;
		}

		/// <summary>
		/// Получение DotNet метрик за заданный промежуток времени
		/// </summary>
		/// <param name="fromTime">Начало временного промежутка</param>
		/// <param name="toTime">Конец временного промежутка</param>
		/// <returns>Список метрик за заданный интервал времени</returns>
		[HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetrics(
			[FromRoute] DateTimeOffset fromTime,
			[FromRoute] DateTimeOffset toTime)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(fromTime)} = {fromTime}" +
				$" {nameof(toTime)} = {toTime}");

			var metrics = _repository.GetByTimeInterval(fromTime, toTime);

			var response = new AllDotNetMetricsResponse()
			{
				Metrics = new List<DotNetMetricDto>()
			};

			foreach (var metric in metrics)
			{
				response.Metrics.Add(_mapper.Map<DotNetMetricDto>(metric));
			}

			return Ok(response);
		}
	}
}
