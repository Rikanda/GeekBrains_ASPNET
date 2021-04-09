using System;
using System.Collections.Generic;

namespace MetricsAgent.Responses
{
	/// <summary>
	/// Контейнер для передачи списка метрик в ответе от сервера
	/// </summary>
	public class AllCpuMetricsResponse
	{
		public List<CpuMetricDto> Metrics { get; set; }
	}

	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class CpuMetricDto
	{
		public TimeSpan Time { get; set; }
		public int Value { get; set; }
		public int Id { get; set; }
	}
}