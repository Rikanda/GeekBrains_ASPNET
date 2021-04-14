using System;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
	
	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class DotNetMetricDto
	{
		public int AgentId { get; set; }
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}



}