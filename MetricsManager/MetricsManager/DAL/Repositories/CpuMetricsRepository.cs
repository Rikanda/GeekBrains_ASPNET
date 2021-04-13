﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using Dapper;
using MetricsManager.SQLsettings;

namespace MetricsManager.DAL
{
	/// <summary>
	/// Маркировочный интерфейс. Необходим, чтобы проверить работу репозитория на тесте-заглушке 
	/// </summary>
	public interface ICpuMetricsRepository : IRepository<CpuMetric>
	{
	}

	/// <summary>
	/// Репозиторий для обработки Cpu метрик
	/// </summary>
	public class CpuMetricsRepository : ICpuMetricsRepository
	{
		/// <summary>
		/// Объект с именами и настройками базы данных
		/// </summary>
		private readonly IMySqlSettings mySql;

		public CpuMetricsRepository(IMySqlSettings mySqlSettings)
		{
			// Добавляем парсилку типа TimeSpan в качестве подсказки для SQLite
			SqlMapper.AddTypeHandler(new TimeSpanHandler());
			mySql = mySqlSettings;
		}

		/// <summary>
		/// Записывает метрику в базу данных
		/// </summary>
		/// <param name="metric">Метрика для записи</param>
		public void Create(CpuMetric metric)
		{
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				connection.ExecuteAsync(
				$"INSERT INTO {mySql[Tables.CpuMetric]}" +
				$"({mySql[Columns.AgentId]}, {mySql[Columns.Value]}, {mySql[Columns.Time]})" +
				$"VALUES (@agentid, @value, @time);",
				new
				{
					value = metric.Value,
					time = metric.Time.TotalSeconds,
					agentId = metric.AgentId,
				});



			}
		}

		/// <summary>
		/// Возвращает список с метриками за заданный интервал времени
		/// </summary>
		/// <param name="agentId">Id агента</param>
		/// <param name="fromTime">Начало временного интервала</param>
		/// <param name="toTime">Конец временного интервала</param>
		/// <returns>Список с метриками за заданный интервал времени</returns>
		public IList<CpuMetric> GetByTimeInterval(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
		{
			var returnList = new List<CpuMetric>();
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				return connection.Query<CpuMetric>(
				"SELECT * " +
				$"FROM {mySql[Tables.CpuMetric]} " +
				$"WHERE (" +
				$"{mySql[Columns.Time]} >= @fromTime " +
				$"AND {mySql[Columns.Time]} <= @toTime " +
				$"AND {mySql[Columns.Time]} == @agentId)",
				new
				{
					fromTime = fromTime.ToUnixTimeSeconds(),
					toTime = toTime.ToUnixTimeSeconds(),
					agentId = agentId,
				}).ToList();
			}
		}

		/// <summary>
		/// Извлекает последнюю по дате метрику из базы данных
		/// </summary>
		/// <returns>Последняя по времени метрика из базы данных</returns>
		public CpuMetric GetLast(int agentId)
		{
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				return connection.QuerySingle<CpuMetric>(
				"SELECT * " +
				$"FROM {mySql[Tables.CpuMetric]} " +
				$"WHERE (" +
				$"{mySql[Columns.Time]} == @agentId " +
				$"AND {mySql[Columns.Time]} = (SELECT MAX ({mySql[Columns.Time]}) FROM {mySql[Tables.CpuMetric]}))",
				new
				{
					agentId = agentId
				});
			}
		}


	}
}