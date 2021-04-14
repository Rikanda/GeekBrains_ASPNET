using Microsoft.AspNetCore.Mvc;
using System;

namespace MetricsManager.Requests
{
	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class RamMetricGetByIntervalForClusterRequest
	{
		[FromRoute]
		public DateTimeOffset fromTime { get; set; }
		[FromRoute]
		public DateTimeOffset toTime { get; set; }
	}

	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class RamMetricGetByIntervalForAgentRequest
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
	public class RamMetricGetByIntervalRequestByClient : IMetricGetByIntervalRequestByClient
	{
		[FromRoute]
		public string agentUri { get; set; }
		[FromRoute]
		public DateTimeOffset fromTime { get; set; }
		[FromRoute]
		public DateTimeOffset toTime { get; set; }
	}

}

