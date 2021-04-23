using MetricsManagerClient.Requests;
using MetricsManagerClient.Responses.FromManager;

namespace MetricsManagerClient.Client
{
	public interface IMetricsClient
	{
		AllAgentsInfoResponse GetAllAgentsInfo();

		AllMetricsResponse<T> GetMetrics<T>(IMetricGetByIntervalRequestByClient request, ApiNames apiName);

	}
}
