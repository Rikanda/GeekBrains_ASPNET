using System;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
	/// <summary>
	/// Контейнер для передачи списка метрик в ответе от сервера
	/// </summary>
	public class AllHddMetricsResponse
	{
		public List<HddMetricDto> Metrics { get; set; }

		public AllHddMetricsResponse()
		{
			Metrics = new List<HddMetricDto>();
		}

	}

	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class HddMetricDto
	{
		public int AgentId { get; set; }
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}


	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class HddMetricFromAgentDto
	{
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}

}