using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Requests
{
	public interface IMetricGetByIntervalRequestByClient
	{
		public string agentUri { get; set; }
		public DateTimeOffset fromTime { get; set; }
		public DateTimeOffset toTime { get; set; }
	}
}
