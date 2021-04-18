using System;

namespace MetricsAgent.DAL.Models
{
	public class RamMetric
	{
		public int Value { get; set; }

		public DateTimeOffset Time { get; set; }
	}
}
