using System;

namespace MetricsAgent.DAL
{
	public class HddMetric
	{
		public int Id { get; set; }

		public int Value { get; set; }

		public TimeSpan Time { get; set; }
	}
}
