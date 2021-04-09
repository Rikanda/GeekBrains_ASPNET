using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using Dapper;

namespace MetricsAgent.DAL
{
	/// <summary>
	/// Маркировочный интерфейс. Необходим, чтобы проверить работу репозитория на тесте-заглушке 
	/// </summary>
	public interface IHddMetricsRepository : IRepository<HddMetric>
	{
	}

	/// <summary>
	/// Репозиторий для обработки Hdd метрик
	/// </summary>
	public class HddMetricsRepository : IHddMetricsRepository
	{
		/// <summary>
		/// Имя таблицы с которой работаем
		/// </summary>
		private const string TableName = "hddmetrics";

		/// <summary>
		/// Строка подключения
		/// </summary>
		private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";

		public HddMetricsRepository()
		{
			// Добавляем парсилку типа TimeSpan в качестве подсказки для SQLite
			SqlMapper.AddTypeHandler(new TimeSpanHandler());
		}

		/// <summary>
		/// Возвращает список с метриками за заданный интервал времени
		/// </summary>
		/// <param name="fromTime">Начало временного интервала</param>
		/// <param name="toTime">Конец временного интервала</param>
		/// <returns>Список с метриками за заданный интервал времени</returns>
		public IList<HddMetric> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
		{
			var returnList = new List<HddMetric>();
			using (var connection = new SQLiteConnection(ConnectionString))
			{
				return connection.Query<HddMetric>(
				"SELECT * " +
				$"FROM {TableName} " +
				"WHERE (time >= @fromTime AND time <= @toTime)",
				new
				{
					fromTime = fromTime.ToUnixTimeSeconds(),
					toTime = toTime.ToUnixTimeSeconds(),
				}).ToList();
			}
		}

		/// <summary>
		/// Извлекает последнюю по дате метрику из базы данных
		/// </summary>
		/// <returns>Последняя по времени метрика из базы данных</returns>
		public HddMetric GetLast()
		{
			using (var connection = new SQLiteConnection(ConnectionString))
			{
				return connection.QuerySingle<HddMetric>(
				"SELECT * " +
				$"FROM {TableName} " +
				$"WHERE time = (SELECT MAX (time) FROM {TableName})");
			}
		}
	}
}
