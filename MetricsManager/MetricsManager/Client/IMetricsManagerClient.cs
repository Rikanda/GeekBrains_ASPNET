using MetricsManager.Requests;
using MetricsManager.Responses;

namespace MetricsManager.Client
{
	public interface IMetricsManagerClient
	{
		AllAgentCpuMetricsResponse GetCpuMetrics(CpuMetricGetByIntervalRequestByClient request);
		AllAgentDotNetMetricsResponse GetDotNetMetrics(DotNetMetricGetByIntervalRequestByClient request);
		AllAgentHddMetricsResponse GetHddMetrics(HddMetricGetByIntervalRequestByClient request);
		AllAgentNetworkMetricsResponse GetNetworkMetrics(NetworkMetricGetByIntervalRequestByClient request);
		AllAgentRamMetricsResponse GetRamMetrics(RamMetricGetByIntervalRequestByClient request);

	}
}
