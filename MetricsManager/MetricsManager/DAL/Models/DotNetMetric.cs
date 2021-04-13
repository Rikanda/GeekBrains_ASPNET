using System;
using System.Collections.Generic;

namespace MetricsManager.DAL
{
	/// <summary>
	/// Контейнер для передачи списка с метриками
	/// </summary>
	public class AllDotNetMetrics
	{
		public List<DotNetMetric> Metrics { get; set; }

		public AllDotNetMetrics()
		{
			Metrics = new List<DotNetMetric>();
		}
	}

	/// <summary>
	/// Контейнер для метрики
	/// </summary>
	public class DotNetMetric
	{
		public int AgentId { get; set; }

		public int Value { get; set; }

		public DateTimeOffset Time { get; set; }
	}
}
