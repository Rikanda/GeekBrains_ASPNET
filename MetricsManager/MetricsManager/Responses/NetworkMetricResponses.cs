using System;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
	/// <summary>
	/// Контейнер для передачи списка метрик в ответе от сервера
	/// </summary>
	public class AllNetworkMetricsResponse
	{
		public List<NetworkMetricDto> Metrics { get; set; }

		public AllNetworkMetricsResponse()
		{
			Metrics = new List<NetworkMetricDto>();
		}

	}

	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class NetworkMetricDto
	{
		public int AgentId { get; set; }
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}


	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class NetworkMetricFromAgentDto
	{
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}

}