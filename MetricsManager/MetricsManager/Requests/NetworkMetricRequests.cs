using Microsoft.AspNetCore.Mvc;
using System;

namespace MetricsManager.Requests
{
	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class NetworkMetricGetByIntervalForClusterRequest
	{
		/// <summary> Начало временного промежутка </summary>
		[FromRoute]
		public DateTimeOffset fromTime { get; set; }

		/// <summary> Конец временного промежутка </summary>
		[FromRoute]
		public DateTimeOffset toTime { get; set; }
	}

	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class NetworkMetricGetByIntervalForAgentRequest
	{
		/// <summary> Id агента </summary>
		[FromRoute]
		public int agentId { get; set; }

		/// <summary> Начало временного промежутка </summary>
		[FromRoute]
		public DateTimeOffset fromTime { get; set; }

		/// <summary> Конец временного промежутка </summary>
		[FromRoute]
		public DateTimeOffset toTime { get; set; }
	}

	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class NetworkMetricGetByIntervalRequestByClient : IMetricGetByIntervalRequestByClient
	{
		/// <summary> URL адрес агента </summary>
		[FromRoute]
		public string agentUri { get; set; }

		/// <summary> Начало временного промежутка </summary>
		[FromRoute]
		public DateTimeOffset fromTime { get; set; }

		/// <summary> Конец временного промежутка </summary>
		[FromRoute]
		public DateTimeOffset toTime { get; set; }
	}

}

