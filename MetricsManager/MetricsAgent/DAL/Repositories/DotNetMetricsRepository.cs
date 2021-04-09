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
	public interface IDotNetMetricsRepository : IRepository<DotNetMetric>
	{
	}

	/// <summary>
	/// Репозиторий для обработки DotNet метрик
	/// </summary>
	public class DotNetMetricsRepository : IDotNetMetricsRepository
	{
		/// <summary>
		/// Имя таблицы с которой работаем
		/// </summary>
		private const string TableName = "dotnetmetrics";

		/// <summary>
		/// Строка подключения
		/// </summary>
		private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";

		public DotNetMetricsRepository()
		{
			// Добавляем парсилку типа TimeSpan в качестве подсказки для SQLite
			SqlMapper.AddTypeHandler(new TimeSpanHandler());
		}

		/// <summary>
		/// Записывает метрику в базу данных
		/// </summary>
		/// <param name="metric">Метрика для записи</param>
		public void Create(DotNetMetric metric)
		{
			using (var connection = new SQLiteConnection(ConnectionString))
			{
				connection.Execute(
				$"INSERT INTO {TableName}" +
				$"(Value, Time)" +
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
		public IList<DotNetMetric> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
		{
			var returnList = new List<DotNetMetric>();
			using (var connection = new SQLiteConnection(ConnectionString))
			{
				return connection.Query<DotNetMetric>(
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
		public DotNetMetric GetLast()
		{
			using (var connection = new SQLiteConnection(ConnectionString))
			{
				return connection.QuerySingle<DotNetMetric>(
				"SELECT * " +
				$"FROM {TableName} " +
				$"WHERE time = (SELECT MAX (time) FROM {TableName})");
			}
		}
	}
}
