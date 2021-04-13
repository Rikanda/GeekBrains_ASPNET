using System;

namespace MetricsManager.DAL
{
	public class RamMetric
	{
		public int AgentId { get; set; }

		public int Value { get; set; }

		public DateTimeOffset Time { get; set; }
	}
}
