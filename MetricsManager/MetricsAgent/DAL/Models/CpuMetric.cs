using System;

namespace MetricsAgent.DAL
{
	public class CpuMetric
	{
		public int Value { get; set; }

		public DateTimeOffset Time { get; set; }
	}
}
