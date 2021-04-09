using System;
using System.Collections.Generic;

namespace MetricsAgent.DAL
{
	public interface IRepository<T> where T : class
	{
		/// <summary>
		/// Извлекает метрики из базы за указанный временной промежуток
		/// </summary>
		/// <param name="fromTime">Начало временного промежутка</param>
		/// <param name="toTime">Конец временного промежутка</param>
		/// <returns>Список метрик за указанный промежуток времени</returns>
		IList<T> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime);

		/// <summary>
		/// Извлекает последнюю собранную метрику из базы данных
		/// </summary>
		/// <returns>Последняя собранная метрика из базы данных</returns>
		T GetLast();

		void Create(T metric);
	}
}
