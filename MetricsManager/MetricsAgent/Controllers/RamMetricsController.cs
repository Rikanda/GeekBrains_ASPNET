using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace MetricsAgent.Controllers
{
	/// <summary>
	/// Контроллер для обработки Ram метрик
	/// </summary>
	[Route("api/metrics/ram")]
	[ApiController]
	public class RamMetricsController : ControllerBase
	{
		private readonly ILogger<RamMetricsController> _logger;
		private readonly IRamMetricsRepository _repository;
		private readonly IMapper _mapper;

		public RamMetricsController(ILogger<RamMetricsController> logger, IRamMetricsRepository repository, IMapper mapper)
		{
			_logger = logger;
			_logger.LogDebug("Вызов конструктора");
			_repository = repository;
			_mapper = mapper;
		}

		/// <summary>
		/// Получение последней собранной Ram метрики из базы данных
		/// </summary>
		/// <returns>Последняя собранная метрика из базы данных</returns>
		[HttpGet("available")]
		public IActionResult GetMetrics()
		{
			_logger.LogDebug("Вызов метода");

			var metric = _repository.GetLast();

			RamMetricDto response = null;

			response = _mapper.Map<RamMetricDto>(metric);

			return Ok(response);
		}
	}
}
