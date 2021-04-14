using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using Dapper;
using MetricsManager.SQLsettings;
using Microsoft.Extensions.Logging;
using Metrics.Tools;

namespace MetricsManager.DAL
{
	/// <summary>
	/// Маркировочный интерфейс. Необходим, чтобы проверить работу репозитория на тесте-заглушке 
	/// </summary>
	public interface IRamMetricsRepository : IRepository<AllRamMetrics>
	{
	}

	/// <summary>
	/// Репозиторий для обработки Cpu метрик
	/// </summary>
	public class RamMetricsRepository : IRamMetricsRepository
	{
		/// <summary>
		/// Объект с именами и настройками базы данных
		/// </summary>
		private readonly IMySqlSettings mySql;
		private readonly ILogger _logger;

		public RamMetricsRepository(IMySqlSettings mySqlSettings, ILogger<RamMetricsRepository> logger)
		{
			// Добавляем парсилку типа DateTimeOffset в качестве подсказки для SQLite
			SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
			mySql = mySqlSettings;
			_logger = logger;
		}

		/// <summary>
		/// Записывает метрику в базу данных
		/// </summary>
		/// <param name="metrics">Список метрик для записи</param>
		public void Create(AllRamMetrics metrics)
		{
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				connection.Open();
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						foreach (var metric in metrics.Metrics)
						{
							connection.ExecuteAsync(
							$"INSERT INTO {mySql[Tables.RamMetric]}" +
							$"({mySql[Columns.AgentId]}, {mySql[Columns.Value]}, {mySql[Columns.Time]})" +
							$"VALUES (@agentid, @value, @time);",
							new
							{
								value = metric.Value,
								time = metric.Time.ToUnixTimeSeconds(),
								agentId = metric.AgentId,
							});
						}
						transaction.Commit();
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						_logger.LogDebug(ex.Message);
					}

				}

			}
		}

		/// <summary>
		/// Возвращает список с метриками за заданный интервал времени
		/// </summary>
		/// <param name="agentId">Id агента</param>
		/// <param name="fromTime">Начало временного интервала</param>
		/// <param name="toTime">Конец временного интервала</param>
		/// <returns>Список с метриками за заданный интервал времени</returns>
		public AllRamMetrics GetByTimeInterval(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
		{
			var metrics = new AllRamMetrics();
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				try
				{
					metrics.Metrics = connection.Query<RamMetric>(
					"SELECT * " +
					$"FROM {mySql[Tables.RamMetric]} " +
					$"WHERE (" +
					$"{mySql[Columns.AgentId]} == @agentId) " +
					$"AND {mySql[Columns.Time]} >= @fromTime " +
					$"AND {mySql[Columns.Time]} <= @toTime ",
					new
					{
						agentId = agentId,
						fromTime = fromTime.ToUnixTimeSeconds(),
						toTime = toTime.ToUnixTimeSeconds(),
					}).ToList();
				}
				catch (Exception ex)
				{
					_logger.LogDebug(ex.Message);
				}
				return metrics;
			}
		}


		public AllRamMetrics GetByTimeIntervalPercentile(
			int agentId,
			DateTimeOffset fromTime,
			DateTimeOffset toTime,
			Percentile percentile)
		{
			var metrics = new AllRamMetrics();
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				try
				{
					metrics.Metrics = connection.Query<RamMetric>(
					"SELECT * " +
					$"FROM {mySql[Tables.RamMetric]} " +
					$"WHERE (" +
					$"{mySql[Columns.AgentId]} == @agentId) " +
					$"AND {mySql[Columns.Time]} >= @fromTime " +
					$"AND {mySql[Columns.Time]} <= @toTime " +
					$"ORDER BY {mySql[Columns.Value]}",
					new
					{
						agentId = agentId,
						fromTime = fromTime.ToUnixTimeSeconds(),
						toTime = toTime.ToUnixTimeSeconds(),
					}).ToList();
				}
				catch (Exception ex)
				{
					_logger.LogDebug(ex.Message);
				}
			}

			var percentileIndex = ((int)percentile * metrics.Metrics.Count / 100);

			var returnMetrics = new AllRamMetrics();
			returnMetrics.Metrics.Add(metrics.Metrics[percentileIndex - 1]);

			return returnMetrics;
		}

		/// <summary>
		/// Извлекает последнюю по дате метрику из базы данных
		/// </summary>
		/// <returns>Последняя по времени метрика из базы данных</returns>
		public AllRamMetrics GetLast(int agentId)
		{
			var metrics = new AllRamMetrics();
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				try
				{
					metrics.Metrics.Add(
						connection.QueryFirst<RamMetric>(
						$"SELECT * " +
						$"FROM {mySql[Tables.RamMetric]} " +
						$"WHERE ({mySql[Columns.AgentId]}, {mySql[Columns.Time]}) " +
						$"IN (SELECT {mySql[Columns.AgentId]}, MAX({mySql[Columns.Time]}) " +
						$"FROM {mySql[Tables.RamMetric]} WHERE {mySql[Columns.AgentId]} == @agentId);",
						new
						{
							agentId = agentId
						}));
				}
				catch (Exception ex)
				{
					_logger.LogDebug(ex.Message);
				}

				return metrics;
			}
		}

	}
}
