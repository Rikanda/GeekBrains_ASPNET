using MetricsManagerClient.Responses.FromManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManagerClient.Models.Interfaces
{
	public interface IAllMetrics<T> where T : class
	{
		int AgentId { get; set; }

		List<T> Metrics { get; set; }

		//Событие при изменении списка метрик
		event EventHandler OnMetricsChange;

		/// <summary>
		/// Последняя сохраненная временная метка в списке
		/// </summary>
		DateTimeOffset LastTime
		{
			get;
		}

		/// <summary>
		/// Добавляет метрики в список
		/// </summary>
		/// <param name="metrics">Набор метрик для добавления</param>
		void AddMetrics(List<T> metrics);

		/// <summary>
		/// Выдает список из значений последних сохраненных метрик
		/// </summary>
		/// <param name="amount">Количество значений которые нужно выдать</param>
		/// <returns>Список значений метрик</returns>
		List<int> GetMetricsValues(int amount);



	}
}
