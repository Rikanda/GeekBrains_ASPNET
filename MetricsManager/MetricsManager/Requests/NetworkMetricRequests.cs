using Microsoft.AspNetCore.Mvc;
using System;

namespace MetricsManager.Requests
{
	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class NetworkMetricGetByIntervalForClusterRequest
	{
		[FromRoute]
		public DateTimeOffset fromTime { get; set; }
		[FromRoute]
		public DateTimeOffset toTime { get; set; }
	}

	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class NetworkMetricGetByIntervalForAgentRequest
	{
		[FromRoute]
		public int agentId { get; set; }
		[FromRoute]
		public DateTimeOffset fromTime { get; set; }
		[FromRoute]
		public DateTimeOffset toTime { get; set; }
	}

	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class NetworkMetricGetByIntervalRequestByClient
	{
		[FromRoute]
		public string agentUri { get; set; }
		[FromRoute]
		public DateTimeOffset fromTime { get; set; }
		[FromRoute]
		public DateTimeOffset toTime { get; set; }
	}

}

