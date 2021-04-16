using Microsoft.AspNetCore.Mvc;
using System;

namespace MetricsManager.Requests
{
	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class RamMetricGetByIntervalForClusterRequest
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
	public class RamMetricGetByIntervalForAgentRequest
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
	public class RamMetricGetByIntervalRequestByClient : IMetricGetByIntervalRequestByClient
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

