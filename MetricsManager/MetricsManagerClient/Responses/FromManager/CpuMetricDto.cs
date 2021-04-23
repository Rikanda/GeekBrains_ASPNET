using System;
using System.Collections.Generic;

namespace MetricsManagerClient.Responses.FromManager
{

	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class CpuMetricDto
	{
		public int AgentId { get; set; }
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}



}