using System;
using System.Collections.Generic;

namespace MetricsAgent.DAL.Interfaces
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
		/// Записывает значение метрики в базу данных
		/// </summary>
		/// <param name="metric">Метрика для занесения в базу данных</param>
		void Create(T metric);
	}
}
