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
	/// Контроллер для обработки Network метрик
	/// </summary>
	[Route("api/metrics/network")]
	[ApiController]
	public class NetworkMetricsController : ControllerBase
	{
		private readonly ILogger<NetworkMetricsController> _logger;
		private readonly INetworkMetricsRepository _repository;
		private readonly IMapper _mapper;

		public NetworkMetricsController(ILogger<NetworkMetricsController> logger, INetworkMetricsRepository repository, IMapper mapper)
		{
			_logger = logger;
			_logger.LogDebug("Вызов конструктора");
			_repository = repository;
			_mapper = mapper;
		}

		/// <summary>
		/// Получение Network метрик за заданный промежуток времени
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

			var response = new AllNetworkMetricsResponse()
			{
				Metrics = new List<NetworkMetricDto>()
			};

			foreach (var metric in metrics)
			{
				response.Metrics.Add(_mapper.Map<NetworkMetricDto>(metric));
			}

			return Ok(response);
		}
	}
}
