using System;

namespace MetricsAgent.DAL
{
	public class NetworkMetric
	{
		public int Value { get; set; }

		public DateTimeOffset Time { get; set; }
	}
}
