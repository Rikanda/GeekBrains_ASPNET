using MetricsManagerClient.Requests;
using MetricsManagerClient.Requests.Interfaces;
using MetricsManagerClient.Responses;
using MetricsManagerClient.Responses.FromManager;

namespace MetricsManagerClient.Client
{
	public interface IMetricsClient
	{
		AllAgentsInfoResponse GetAllAgentsInfo();

		AllMetricsResponse<T> GetMetrics<T>(IMetricGetByIntervalRequestByClient request, ApiNames apiName);

	}
}
