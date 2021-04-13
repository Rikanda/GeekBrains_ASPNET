using MetricsManager.Responses;
using MetricsManager.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.IO;

namespace MetricsManager.Client
{
	public class MetricsManagerClient : IMetricsManagerClient
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger _logger;


		public MetricsManagerClient(HttpClient httpClient, ILogger<MetricsManagerClient> logger)
		{
			_httpClient = httpClient;
			_logger = logger;
		}

		public AllAgentCpuMetricsResponse GetCpuMetrics(CpuMetricGetByIntervalRequestByClient request)
		{
			var fromParameter = request.fromTime.ToString("O");
			var toParameter = request.toTime.ToString("O");
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get, 
				$"{request.agentUri}/api/metrics/cpu/from/{fromParameter}/to/{toParameter}");
			try
			{
				HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

				using var responseStream = response.Content.ReadAsStreamAsync().Result;
				using var streamReader = new StreamReader(responseStream);
				var content = streamReader.ReadToEnd();

				var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
				var returnResp = JsonSerializer.Deserialize<AllAgentCpuMetricsResponse>(content, options);
				return returnResp;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return null;
		}
	}
}
