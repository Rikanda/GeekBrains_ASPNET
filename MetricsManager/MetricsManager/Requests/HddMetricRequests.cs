using Microsoft.AspNetCore.Mvc;
using System;

namespace MetricsManager.Requests
{
	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class HddMetricGetByIntervalForClusterRequest
	{
		[FromRoute]
		public DateTimeOffset fromTime { get; set; }
		[FromRoute]
		public DateTimeOffset toTime { get; set; }
	}

	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class HddMetricGetByIntervalForAgentRequest
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
	public class HddMetricGetByIntervalRequestByClient
	{
		[FromRoute]
		public string agentUri { get; set; }
		[FromRoute]
		public DateTimeOffset fromTime { get; set; }
		[FromRoute]
		public DateTimeOffset toTime { get; set; }
	}

}

