using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using Dapper;
using MetricsAgent.MySQLsettings;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;

namespace MetricsAgent.DAL.Repositories
{
	/// <summary>
	/// Маркировочный интерфейс. Необходим, чтобы проверить работу репозитория на тесте-заглушке 
	/// </summary>
	public interface IDotNetMetricsRepository : IRepository<DotNetMetric>
	{
	}

	/// <summary>
	/// Репозиторий для обработки DotNet метрик
	/// </summary>
	public class DotNetMetricsRepository : IDotNetMetricsRepository
	{
		/// <summary>
		/// Объект с именами и настройками базы данных
		/// </summary>
		private readonly IMySqlSettings mySql;

		public DotNetMetricsRepository(IMySqlSettings mySqlSettings)
		{
			// Добавляем парсилку типа DateTimeOffset в качестве подсказки для SQLite
			SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
			mySql = mySqlSettings;
		}

		/// <summary>
		/// Записывает метрику в базу данных
		/// </summary>
		/// <param name="metric">Метрика для записи</param>
		public void Create(DotNetMetric metric)
		{
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				connection.ExecuteAsync(
				$"INSERT INTO {mySql[Tables.DotNetMetric]}" +
				$"({mySql[Columns.Value]}, {mySql[Columns.Time]})" +
				$"VALUES (@value, @time);",
				new
				{
					value = metric.Value,
					time = metric.Time.ToUnixTimeSeconds(),
				});
			}
		}

		/// <summary>
		/// Возвращает список с метриками за заданный интервал времени
		/// </summary>
		/// <param name="fromTime">Начало временного интервала</param>
		/// <param name="toTime">Конец временного интервала</param>
		/// <returns>Список с метриками за заданный интервал времени</returns>
		public IList<DotNetMetric> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
		{
			var returnList = new List<DotNetMetric>();
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				return connection.Query<DotNetMetric>(
				"SELECT * " +
				$"FROM {mySql[Tables.DotNetMetric]} " +
				$"WHERE ({mySql[Columns.Time]} >= @fromTime AND {mySql[Columns.Time]} <= @toTime)",
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
		public DotNetMetric GetLast()
		{
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				return connection.QuerySingle<DotNetMetric>(
				"SELECT * " +
				$"FROM {mySql[Tables.DotNetMetric]} " +
				$"WHERE {mySql[Columns.Time]} = (SELECT MAX ({mySql[Columns.Time]}) FROM {mySql[Tables.DotNetMetric]})");
			}
		}


	}
}
