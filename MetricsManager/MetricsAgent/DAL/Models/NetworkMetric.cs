using System;

namespace MetricsAgent.DAL.Models
{
	public class NetworkMetric
	{
		public int Value { get; set; }

		public DateTimeOffset Time { get; set; }
	}
}
