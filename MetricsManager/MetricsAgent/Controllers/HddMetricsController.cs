using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;

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
		/// Получение последней собранной Hdd метрики из базы данных
		/// </summary>
		/// <returns>Последняя собранная метрика из базы данных</returns>
		[HttpGet("agent/left")]
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
