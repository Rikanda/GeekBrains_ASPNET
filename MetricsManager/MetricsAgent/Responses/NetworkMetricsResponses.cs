using System;
using System.Collections.Generic;

namespace MetricsAgent.Responses
{
	/// <summary>
	/// Контейнер для передачи списка метрик в ответе от сервера
	/// </summary>
	public class AllNetworkMetricsResponse
	{
		public List<NetworkMetricDto> Metrics { get; set; }
	}

	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class NetworkMetricDto
	{
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}
}
