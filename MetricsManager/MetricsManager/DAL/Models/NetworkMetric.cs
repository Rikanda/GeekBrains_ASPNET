using System;
using System.Collections.Generic;

namespace MetricsManager.DAL.Models
{


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
