using MetricsManager.Requests;
using MetricsManager.Responses;

namespace MetricsManager.Client
{
	public interface IMetricsManagerClient
	{
		AllAgentCpuMetricsResponse GetCpuMetrics(CpuMetricGetByIntervalRequestByClient request);

	}
}
