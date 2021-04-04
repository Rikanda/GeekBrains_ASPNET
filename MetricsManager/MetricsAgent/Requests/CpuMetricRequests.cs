using System;

namespace MetricsAgent.Requests
{
	/// <summary>
	/// Контейнер для запроса на добаление метрики в базу
	/// </summary>
	public class CpuMetricCreateRequest
	{
		public int Time { get; set; }
		public int Value { get; set; }
	}
}