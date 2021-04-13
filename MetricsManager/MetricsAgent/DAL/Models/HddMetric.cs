using System;

namespace MetricsAgent.DAL
{
	public class HddMetric
	{
		public int Value { get; set; }

		public DateTimeOffset Time { get; set; }
	}
}
