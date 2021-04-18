using MetricsManager.Requests.Interfaces;
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
		public DateTimeOffset FromTime { get; set; }
		[FromRoute]
		public DateTimeOffset ToTime { get; set; }
	}

	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class CpuMetricGetByIntervalForAgentRequest
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
	public class CpuMetricGetByIntervalRequestByClient : IMetricGetByIntervalRequestByClient
	{
		[FromRoute]
		public string AgentUri { get; set; }
		[FromRoute]
		public DateTimeOffset FromTime { get; set; }
		[FromRoute]
		public DateTimeOffset ToTime { get; set; }
	}

}

