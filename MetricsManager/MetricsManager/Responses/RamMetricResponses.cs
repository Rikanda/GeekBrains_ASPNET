using System;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
	/// <summary>
	/// Контейнер для передачи списка метрик в ответе от сервера
	/// </summary>
	public class AllRamMetricsResponse
	{
		public List<RamMetricDto> Metrics { get; set; }

		public AllRamMetricsResponse()
		{
			Metrics = new List<RamMetricDto>();
		}

	}

	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class RamMetricDto
	{
		public int AgentId { get; set; }
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}


	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class RamMetricFromAgentDto
	{
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}

}