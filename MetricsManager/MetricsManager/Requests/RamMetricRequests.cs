using MetricsManager.Requests.Interfaces;
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
		public DateTimeOffset FromTime { get; set; }
		[FromRoute]
		public DateTimeOffset ToTime { get; set; }
	}

	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class RamMetricGetByIntervalForAgentRequest
	{
		[FromRoute]
		public int AgentId { get; set; }
		[FromRoute]
		public DateTimeOffset FromTime { get; set; }
		[FromRoute]
		public DateTimeOffset ToTime { get; set; }
	}

	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class RamMetricGetByIntervalRequestByClient : IMetricGetByIntervalRequestByClient
	{
		[FromRoute]
		public string AgentUri { get; set; }
		[FromRoute]
		public DateTimeOffset FromTime { get; set; }
		[FromRoute]
		public DateTimeOffset ToTime { get; set; }
	}

}

