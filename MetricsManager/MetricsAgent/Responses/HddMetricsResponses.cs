using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Responses
{
	public class HddMetricsResponses
	{
		public class AllHddMetricsResponse
		{
			public List<HddMetricDto> Metrics { get; set; }
		}

		public class HddMetricDto
		{
			public TimeSpan Time { get; set; }
			public int Value { get; set; }
			public int Id { get; set; }
		}
	}
}
