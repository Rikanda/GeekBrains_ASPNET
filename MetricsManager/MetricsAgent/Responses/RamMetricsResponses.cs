using System;
using System.Collections.Generic;

namespace MetricsAgent.Responses
{
	/// <summary>
	/// Контейнер для передачи списка метрик в ответе от сервера
	/// </summary>
	public class AllRamMetricsResponse
	{
		public List<RamMetricDto> Metrics { get; set; }
	}

	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class RamMetricDto
	{
		public TimeSpan Time { get; set; }
		public int Value { get; set; }
	}
}
