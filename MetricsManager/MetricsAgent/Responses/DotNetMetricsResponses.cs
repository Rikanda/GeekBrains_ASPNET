using System;
using System.Collections.Generic;

namespace MetricsAgent.Responses
{
	/// <summary>
	/// Контейнер для передачи списка метрик в ответе от сервера
	/// </summary>
	public class AllDotNetMetricsResponse
	{
		public List<DotNetMetricDto> Metrics { get; set; }
	}

	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class DotNetMetricDto
	{
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}
}
