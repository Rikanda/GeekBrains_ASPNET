using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using MetricsManagerClient.Requests.Interfaces;
using MetricsManagerClient.Responses.FromManager;

namespace MetricsManagerClient.Client
{
	public enum ApiNames
	{
		Cpu,
		DotNet,
		Hdd,
		Network,
		Ram,
	}

	public class MetricsClient : IMetricsClient
	{
		private readonly string ManagerUri = "http://localhost:5000";

		private readonly HttpClient _httpClient;
		private readonly ILogger _logger;

		private readonly Dictionary<ApiNames, string> apiNames = new Dictionary<ApiNames, string>()
		{
			{ApiNames.Cpu, "cpu" },
			{ApiNames.DotNet, "dotnet" },
			{ApiNames.Hdd, "hdd" },
			{ApiNames.Network, "network" },
			{ApiNames.Ram, "ram" },
		};


		public MetricsClient(HttpClient httpClient, ILogger<MetricsClient> logger)
		{
			_httpClient = httpClient;
			_logger = logger;
		}

		public AllAgentsInfoResponse GetAllAgentsInfo()
		{
			var allAgentsInfo = new AllAgentsInfoResponse();

			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get,
				$"{ManagerUri}/api/agents/read");

			try
			{
				HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

				var responseStream = response.Content.ReadAsStreamAsync().Result;
				var streamReader = new StreamReader(responseStream);
				var content = streamReader.ReadToEnd();

				var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
				var returnResponse = JsonSerializer.Deserialize<AllAgentsInfoResponse>(content, options);
				return returnResponse;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return null;
		}

		public AllMetricsResponse<T> GetMetrics<T>(IMetricGetByIntervalRequestByClient request, ApiNames apiName)
		{
			var fromParameter = request.FromTime.UtcDateTime.ToString("O");
			var toParameter = request.ToTime.UtcDateTime.ToString("O");
			var agentId = request.AgentId.ToString();
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get,
				$"{ManagerUri}/api/metrics/{apiNames[apiName]}/agent/{agentId}/from/{fromParameter}/to/{toParameter}");

			try
			{
				HttpResponseMessage managerResponse = _httpClient.SendAsync(httpRequest).Result;

				var responseStream = managerResponse.Content.ReadAsStreamAsync().Result;
				var streamReader = new StreamReader(responseStream);
				var content = streamReader.ReadToEnd();

				var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
				var response = JsonSerializer.Deserialize<AllMetricsResponse<T>>(content, options);
				return response;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return null;
		}

	}
}
