using System;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
	/// <summary>
	/// Контейнер для передачи списка метрик в ответе от сервера
	/// </summary>
	public class AllDotNetMetricsResponse
	{
		public List<DotNetMetricDto> Metrics { get; set; }

		public AllDotNetMetricsResponse()
		{
			Metrics = new List<DotNetMetricDto>();
		}

	}

	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class DotNetMetricDto
	{
		public int AgentId { get; set; }
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}

	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class DotNetMetricFromAgentDto
	{
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}

}