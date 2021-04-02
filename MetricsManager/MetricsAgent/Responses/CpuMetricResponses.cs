using System;
using System.Collections.Generic;

namespace MetricsAgent.Responses
{
	public class AllCpuMetricsResponse
	{
		public List<CpuMetricDto> Metrics { get; set; }
	}

	public class CpuMetricDto
	{
		public TimeSpan Time { get; set; }
		public int Value { get; set; }
		public int Id { get; set; }
	}
}