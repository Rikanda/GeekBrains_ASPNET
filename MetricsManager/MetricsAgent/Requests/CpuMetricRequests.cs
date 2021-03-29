using System;

namespace MetricsAgent.Requests
{
	public class CpuMetricCreateRequest
	{
		public int Time { get; set; }
		public int Value { get; set; }
	}
}