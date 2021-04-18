using System;

namespace MetricsAgent.DAL.Models
{
	public class CpuMetric
	{
		public int Value { get; set; }

		public DateTimeOffset Time { get; set; }
	}
}
