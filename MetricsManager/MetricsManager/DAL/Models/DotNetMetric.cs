using System;

namespace MetricsManager.DAL
{
	public class DotNetMetric
	{
		public int AgentId { get; set; }

		public int Value { get; set; }

		public TimeSpan Time { get; set; }
	}
}
