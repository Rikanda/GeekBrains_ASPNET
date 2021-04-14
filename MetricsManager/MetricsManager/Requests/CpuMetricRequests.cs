using Microsoft.AspNetCore.Mvc;
using System;

namespace MetricsManager.Requests
{
	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class CpuMetricGetByIntervalForClusterRequest
	{
		[FromRoute]
		public DateTimeOffset fromTime { get; set; }
		[FromRoute]
		public DateTimeOffset toTime { get; set; }
	}

	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class CpuMetricGetByIntervalForAgentRequest
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
	public class CpuMetricGetByIntervalRequestByClient : IMetricGetByIntervalRequestByClient
	{
		[FromRoute]
		public string agentUri { get; set; }
		[FromRoute]
		public DateTimeOffset fromTime { get; set; }
		[FromRoute]
		public DateTimeOffset toTime { get; set; }
	}

}

