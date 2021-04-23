using MetricsManagerClient.Models.Interfaces;
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

	public interface IAllRamMetrics : IAllMetrics<RamMetric>
	{
	}

	public class AllRamMetrics : IAllRamMetrics
	{
		/// <summary>Количество метрик хранящихся в классе</summary>
		private readonly int Amount = 200;

		private readonly ILogger _logger;

		public int AgentId { get; set; }

		public List<RamMetric> Metrics { get; set; }

		//Событие при изменении списка метрик
		public event EventHandler OnMetricsChange;

		public AllRamMetrics(ILogger<AllRamMetrics> logger)
		{
			_logger = logger;
			AgentId = 1;

			Metrics = new List<RamMetric>();

			//Заполнение списка метрик пустыми значениями
			var newMetric = new RamMetric() { Time = DateTimeOffset.UtcNow, Value = 0 };
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
		public void AddMetrics(List<RamMetric> metrics)
		{
			_logger.LogDebug("Adding metrics ");

			//Убираем дубликат последней метрике если он есть
			if (metrics.Count != 0)
			{
				if (metrics[0].Time == Metrics.Last().Time)
				{
					metrics.RemoveAt(0);
				}
			}

			//Если еще остались новые метрики, то заносим их в список, удаля старые в начале
			foreach (var metric in metrics)
			{
				Metrics.Add(metric);
				Metrics.RemoveAt(0);
			}

			//Отправляем сообщение о том, что список метрик изменился
			OnMetricsChange(this, new EventArgs());
			_logger.LogDebug($"Added {metrics.Count} metrics");
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
			if (amount > Amount)
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
