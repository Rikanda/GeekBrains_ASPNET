using MetricsManagerClient.Requests;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MetricsManagerClient.Requests
{
	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class RamMetricGetByIntervalRequestByClient : IMetricGetByIntervalRequestByClient
	{
		/// <summary> URL адрес агента </summary>
		[FromRoute]
		public int AgentId { get; set; }

		/// <summary> Начало временного промежутка </summary>
		[FromRoute]
		public DateTimeOffset FromTime { get; set; }

		/// <summary> Конец временного промежутка </summary>
		[FromRoute]
		public DateTimeOffset ToTime { get; set; }
	}

}

