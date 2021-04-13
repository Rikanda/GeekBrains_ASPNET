using System;
using System.Collections.Generic;

namespace MetricsManager.DAL
{
	/// <summary>
	/// Контейнер для передачи списка с метриками
	/// </summary>
	public class AllNetworkMetrics
	{
		public List<NetworkMetric> Metrics { get; set; }

		public AllNetworkMetrics()
		{
			Metrics = new List<NetworkMetric>();
		}
	}

	/// <summary>
	/// Контейнер для метрики
	/// </summary>
	public class NetworkMetric
	{
		public int AgentId { get; set; }

		public int Value { get; set; }

		public DateTimeOffset Time { get; set; }
	}
}
