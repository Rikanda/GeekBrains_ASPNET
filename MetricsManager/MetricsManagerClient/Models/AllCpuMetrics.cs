using MetricsManagerClient.Models.Metrics;
using MetricsManagerClient.Responses.FromManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManagerClient.Models
{
	public class AllCpuMetrics
	{
		/// <summary>Количество метрик хранящихся в классе</summary>
		private readonly int Amount = 200;

		private readonly ILogger _logger;

		public int AgentId { get; set; }

		public List<CpuMetric> Metrics { get; set; }

		//Делегат для события при изменении списка метрик
		public delegate void MethodContainer();

		//Событие при изменении списка метрик
		public event MethodContainer onMetricsChange;

		public AllCpuMetrics(ILogger<AllCpuMetrics> logger)
		{
			_logger = logger;
			AgentId = 1;

			Metrics = new List<CpuMetric>();

			//Заполнение списка метрик пустыми значениями
			var newMetric = new CpuMetric() { Time = DateTimeOffset.UtcNow, Value = 0 };
			for (int i = 0; i < Amount; i++)
			{
				Metrics.Add(newMetric);
				newMetric.Time -= TimeSpan.FromSeconds(5);
			}
		}

		/// <summary>
		/// Последняя сохраненная временная метка в списке
		/// </summary>
		public DateTimeOffset LastTime
		{
			get
			{
				return Metrics.Last().Time; ;
			}
		}

		/// <summary>
		/// Добавляет метрики в список
		/// </summary>
		/// <param name="metrics">Набор метрик для добавления</param>
		public void AddMetrics(AllMetricsResponse<CpuMetricDto> metrics)
		{
			_logger.LogDebug("Adding metrics ");

			//Убираем дубликат последней метрике если он есть
			if (metrics.Metrics.Count != 0)
			{
				if(metrics.Metrics[0].Time == Metrics.Last().Time)
				{
					metrics.Metrics.RemoveAt(0);
				}
			}

			//Если еще остались новые метрики, то заносим их в список, удаля старые в начале
			if (metrics.Metrics.Count != 0)
			{
				foreach (var metric in metrics.Metrics)
				{
					Metrics.Add(new CpuMetric() { Value = metric.Value, Time = metric.Time });
					Metrics.RemoveAt(0);
				}

				//Отправляем сообщение о том, что список метрик изменился
				onMetricsChange();
				_logger.LogDebug($"Added {metrics.Metrics.Count} metrics");
			}
		}

		/// <summary>
		/// Выдает список из значений последних сохраненных метрик
		/// </summary>
		/// <param name="amount">Количество значений которые нужно выдать</param>
		/// <returns>Список значений метрик</returns>
		public List<int> GetMetricsValues(int amount)
		{
			var newValuesList = new List<int>();

			//Проверка на то что запрашиваемое количество не больше того, которое хранится в списке
			if(amount > Amount)
			{
				amount = Amount;
			}
			_logger.LogDebug($"Sended {amount} metrics");

			//Составляем список который вернем по запросу
			for (int i = Metrics.Count - amount; i < Metrics.Count; i++)
			{
				newValuesList.Add(Metrics[i].Value);
			}

			return newValuesList;
		}

	}
}
