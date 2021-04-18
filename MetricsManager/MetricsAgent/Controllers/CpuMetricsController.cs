using AutoMapper;
using MetricsAgent.DAL;
using MetricsAgent.DAL.Repositories;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

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
		/// <param name="request">Запрос на выдачу метрик с интервалом времени</param>
		/// <returns>Список метрик за заданный интервал времени</returns>
		[HttpGet("from/{request.fromTime}/to/{request.toTime}")]
		public IActionResult GetMetrics([FromRoute]CpuMetricGetByIntervalRequest request)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.FromTime)} = {request.FromTime}" +
				$" {nameof(request.ToTime)} = {request.ToTime}");

			var metrics = _repository.GetByTimeInterval(request.FromTime, request.ToTime);

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

	}
}
