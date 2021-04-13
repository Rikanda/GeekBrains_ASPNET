using System;
using System.Collections.Generic;

namespace MetricsManager.DAL
{
	/// <summary>
	/// Контейнер для передачи списка с метриками
	/// </summary>
	public class AllCpuMetrics
	{
		public List<CpuMetric> Metrics { get; set; }

		public AllCpuMetrics()
		{
			Metrics = new List<CpuMetric>();
		}
	}

	/// <summary>
	/// Контейнер для метрики
	/// </summary>
	public class CpuMetric
	{
		public int AgentId { get; set; }

		public int Value { get; set; }

		public DateTimeOffset Time { get; set; }
	}
}
