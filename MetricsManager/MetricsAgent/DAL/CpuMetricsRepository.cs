using MetricsAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;
using MetricsAgent.DAL;

namespace MetricsAgent.DAL
{
	//Маркировочный интерфейс
	//Необходим, чтобы проверить работу репозитория на тесте-заглушке
	public interface ICpuMetricsRepository : IRepository<CpuMetric>
	{
	}

	public class CpuMetricsRepository : ICpuMetricsRepository
	{
		//Имя таблицы с которой работаем
		private const string TableName = "cpumetrics";

		//Строка подключения
		private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";

		//Инжектируем соединение с базой данных в наш репозиторий через конструктор
		public CpuMetricsRepository()
		{
			//Добавляем парсилку типа TimeSpan в качестве подсказки для SQLite
			SqlMapper.AddTypeHandler(new TimeSpanHandler());
		}

		/// <summary>
		/// Возвращает список с метриками за заданный интервал времени
		/// </summary>
		/// <param name="fromTime">Начало временного интервала</param>
		/// <param name="toTime">Конец временного интервала</param>
		/// <returns>Список с метриками за заданный интервал времени</returns>
		public IList<CpuMetric> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
		{
			var returnList = new List<CpuMetric>();
			using (var connection = new SQLiteConnection(ConnectionString))
			{
				return connection.Query<CpuMetric>(
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
		public CpuMetric GetLast()
		{
			using (var connection = new SQLiteConnection(ConnectionString))
			{
				return connection.QuerySingle<CpuMetric>(
				"SELECT * " +
				$"FROM {TableName} " +
				$"WHERE time = (SELECT MAX (time) FROM {TableName})");
			}
		}
	}
}
