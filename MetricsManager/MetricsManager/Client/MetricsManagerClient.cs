using MetricsManager.Responses;
using MetricsManager.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

namespace MetricsManager.Client
{

	public class MetricsManagerClient : IMetricsManagerClient
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger _logger;

		public enum ApiNames
		{
			Cpu,
			DotNet,
			Hdd,
			Network,
			Ram,
		}

		private readonly Dictionary<ApiNames, string> apiNames = new Dictionary<ApiNames, string>()
		{
			{ApiNames.Cpu, "cpu" },
			{ApiNames.DotNet, "dotnet" },
			{ApiNames.Hdd, "hdd" },
			{ApiNames.Network, "network" },
			{ApiNames.Ram, "ram" },
		};


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

		public AllAgentDotNetMetricsResponse GetDotNetMetrics(DotNetMetricGetByIntervalRequestByClient request)
		{
			var fromParameter = request.fromTime.ToString("O");
			var toParameter = request.toTime.ToString("O");
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get,
				$"{request.agentUri}/api/metrics/dotnet/from/{fromParameter}/to/{toParameter}");

			try
			{
				HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

				using var responseStream = response.Content.ReadAsStreamAsync().Result;
				using var streamReader = new StreamReader(responseStream);
				var content = streamReader.ReadToEnd();

				var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
				var returnResp = JsonSerializer.Deserialize<AllAgentDotNetMetricsResponse>(content, options);
				return returnResp;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return null;
		}

		public AllAgentHddMetricsResponse GetHddMetrics(HddMetricGetByIntervalRequestByClient request)
		{
			var fromParameter = request.fromTime.ToString("O");
			var toParameter = request.toTime.ToString("O");
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get,
				$"{request.agentUri}/api/metrics/hdd/from/{fromParameter}/to/{toParameter}");

			try
			{
				HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

				using var responseStream = response.Content.ReadAsStreamAsync().Result;
				using var streamReader = new StreamReader(responseStream);
				var content = streamReader.ReadToEnd();

				var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
				var returnResp = JsonSerializer.Deserialize<AllAgentHddMetricsResponse>(content, options);
				return returnResp;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return null;
		}

		public AllAgentNetworkMetricsResponse GetNetworkMetrics(NetworkMetricGetByIntervalRequestByClient request)
		{
			var fromParameter = request.fromTime.ToString("O");
			var toParameter = request.toTime.ToString("O");
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get,
				$"{request.agentUri}/api/metrics/network/from/{fromParameter}/to/{toParameter}");

			try
			{
				HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

				using var responseStream = response.Content.ReadAsStreamAsync().Result;
				using var streamReader = new StreamReader(responseStream);
				var content = streamReader.ReadToEnd();

				var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
				var returnResp = JsonSerializer.Deserialize<AllAgentNetworkMetricsResponse>(content, options);
				return returnResp;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return null;
		}

		public AllAgentRamMetricsResponse GetRamMetrics(RamMetricGetByIntervalRequestByClient request)
		{
			var fromParameter = request.fromTime.ToString("O");
			var toParameter = request.toTime.ToString("O");
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get,
				$"{request.agentUri}/api/metrics/ram/from/{fromParameter}/to/{toParameter}");

			try
			{
				HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

				using var responseStream = response.Content.ReadAsStreamAsync().Result;
				using var streamReader = new StreamReader(responseStream);
				var content = streamReader.ReadToEnd();

				var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
				var returnResp = JsonSerializer.Deserialize<AllAgentRamMetricsResponse>(content, options);
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
