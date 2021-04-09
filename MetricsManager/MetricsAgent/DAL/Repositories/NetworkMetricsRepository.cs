using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using Dapper;
using MetricsAgent.SQLsettings;

namespace MetricsAgent.DAL
{
	/// <summary>
	/// Маркировочный интерфейс. Необходим, чтобы проверить работу репозитория на тесте-заглушке 
	/// </summary>
	public interface INetworkMetricsRepository : IRepository<NetworkMetric>
	{
	}

	/// <summary>
	/// Репозиторий для обработки Network метрик
	/// </summary>
	public class NetworkMetricsRepository : INetworkMetricsRepository
	{
		/// <summary>
		/// Объект с именами и настройками базы данных
		/// </summary>
		private readonly IMySqlSettings mySql;

		public NetworkMetricsRepository(IMySqlSettings mySqlSettings)
		{
			// Добавляем парсилку типа TimeSpan в качестве подсказки для SQLite
			SqlMapper.AddTypeHandler(new TimeSpanHandler());
			mySql = mySqlSettings;
		}

		/// <summary>
		/// Записывает метрику в базу данных
		/// </summary>
		/// <param name="metric">Метрика для записи</param>
		public void Create(NetworkMetric metric)
		{
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				connection.Execute(
				$"INSERT INTO {mySql[Tables.NetworkMetric]}" +
				$"({mySql[Rows.Value]}, {mySql[Rows.Time]})" +
				$"VALUES (@value, @time);",
				new
				{
					value = metric.Value,
					time = metric.Time.TotalSeconds,
				});
			}
		}

		/// <summary>
		/// Возвращает список с метриками за заданный интервал времени
		/// </summary>
		/// <param name="fromTime">Начало временного интервала</param>
		/// <param name="toTime">Конец временного интервала</param>
		/// <returns>Список с метриками за заданный интервал времени</returns>
		public IList<NetworkMetric> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
		{
			var returnList = new List<NetworkMetric>();
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				return connection.Query<NetworkMetric>(
				"SELECT * " +
				$"FROM {mySql[Tables.NetworkMetric]} " +
				$"WHERE ({mySql[Rows.Time]} >= @fromTime AND {mySql[Rows.Time]} <= @toTime)",
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
		public NetworkMetric GetLast()
		{
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				return connection.QuerySingle<NetworkMetric>(
				"SELECT * " +
				$"FROM {mySql[Tables.NetworkMetric]} " +
				$"WHERE {mySql[Rows.Time]} = (SELECT MAX ({mySql[Rows.Time]}) FROM {mySql[Tables.NetworkMetric]})");
			}
		}


	}
}
