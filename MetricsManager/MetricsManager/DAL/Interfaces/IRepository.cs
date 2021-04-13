using System;
using System.Collections.Generic;

namespace MetricsManager.DAL
{
	public interface IRepository<T> where T : class
	{
		/// <summary>
		/// Извлекает метрики из базы за указанный временной промежуток
		/// </summary>
		/// <param name="agentId">Id агента</param>
		/// <param name="fromTime">Начало временного промежутка</param>
		/// <param name="toTime">Конец временного промежутка</param>
		/// <returns>Список метрик за указанный промежуток времени</returns>
		T GetByTimeInterval(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime);

		/// <summary>
		/// Извлекает последнюю собранную метрику из базы данных
		/// </summary>
		/// <param name="agentId">Id агента</param>
		/// <returns>Последняя собранная метрика из базы данных</returns>
		T GetLast(int agentId);

		/// <summary>
		/// Записывает значение метрики в базу данных
		/// </summary>
		/// <param name="metrics">Метрика для занесения в базу данных</param>
		void Create(T metrics);
	}
}
