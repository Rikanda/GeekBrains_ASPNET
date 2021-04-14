using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Responses
{
	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class DotNetMetricFromAgentDto
	{
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}
}
