using System;

namespace MetricsAgent.DAL.Models
{
	public class DotNetMetric
	{
		public int Value { get; set; }

		public DateTimeOffset Time { get; set; }
	}
}
