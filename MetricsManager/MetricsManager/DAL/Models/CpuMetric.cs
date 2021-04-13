using System;
using System.Collections.Generic;

namespace MetricsManager.DAL
{
	/// <summary>
	/// Контейнер для передачи списка с информацие об агентах
	/// </summary>
	public class AllCpuMetrics
	{
		public List<CpuMetric> Metrics { get; set; }
	}

	public class CpuMetric
	{
		public int AgentId { get; set; }

		public int Value { get; set; }

		public DateTimeOffset Time { get; set; }
	}
}
