using System;

namespace MetricsManagerClient.Requests
{
	public interface IMetricGetByIntervalRequestByClient
	{
		int AgentId { get; set; }
		DateTimeOffset FromTime { get; set; }
		DateTimeOffset ToTime { get; set; }
	}
}
