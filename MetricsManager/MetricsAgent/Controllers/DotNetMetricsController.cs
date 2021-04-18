using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using AutoMapper;
using MetricsAgent.Requests;
using MetricsAgent.DAL.Repositories;

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
		/// <param name="request">Запрос на выдачу метрик с интервалом времени</param>
		/// <returns>Список метрик за заданный интервал времени</returns>
		[HttpGet("from/{FromTime}/to/{ToTime}")]
		public IActionResult GetMetrics([FromRoute] DotNetMetricGetByIntervalRequest request)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.FromTime)} = {request.FromTime}" +
				$" {nameof(request.ToTime)} = {request.ToTime}");

			var metrics = _repository.GetByTimeInterval(request.FromTime, request.ToTime);

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
