using MetricsManager.Requests;
using MetricsManager.Responses;

namespace MetricsManager.Client
{
	public interface IMetricsManagerClient
	{
		AllAgentMetricsResponse<T> GetMetrics<T>(IMetricGetByIntervalRequestByClient request, ApiNames apiName);

	}
}
