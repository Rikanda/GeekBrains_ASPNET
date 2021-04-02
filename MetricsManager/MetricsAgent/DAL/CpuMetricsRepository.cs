using MetricsAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MetricsAgent.DAL
{
	// маркировочный интерфейс
	// необходим, чтобы проверить работу репозитория на тесте-заглушке
	public interface ICpuMetricsRepository : IRepository<CpuMetric>
	{
	}

	public class CpuMetricsRepository : ICpuMetricsRepository
	{
		// наше соединение с базой данных
		private readonly SQLiteConnection connection;

		// инжектируем соединение с базой данных в наш репозиторий через конструктор
		public CpuMetricsRepository(SQLiteConnection connection)
		{
			this.connection = connection;
		}

		public IList<CpuMetric> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
		{
			using var cmd = new SQLiteCommand(connection);

			// прописываем в команду SQL запрос на получение всех данных из таблицы
			cmd.CommandText = "SELECT * FROM cpumetrics WHERE (time >= @fromtime AND time <= @totime)";
			cmd.Parameters.AddWithValue("@fromtime", fromTime.ToUnixTimeSeconds());
			cmd.Parameters.AddWithValue("@totime", toTime.ToUnixTimeSeconds());

			var returnList = new List<CpuMetric>();

			using (SQLiteDataReader reader = cmd.ExecuteReader())
			{
				// пока есть что читать -- читаем
				while (reader.Read())
				{
					// добавляем объект в список возврата
					returnList.Add(new CpuMetric
					{
						Id = reader.GetInt32(0),
						Value = reader.GetInt32(1),
						Time = TimeSpan.FromSeconds(reader.GetInt32(2))
					});
				}
			}

			return returnList;
		}

		public CpuMetric GetLast()
		{
			using var cmd = new SQLiteCommand(connection);

			// прописываем в команду SQL запрос на получение всех данных из таблицы
			cmd.CommandText = "SELECT * FROM cpumetrics WHERE (time = (SELECT MAX(time) FROM cpumetrics))";

			var returnItem = new CpuMetric();

			using (SQLiteDataReader reader = cmd.ExecuteReader())
			{
				// пока есть что читать -- читаем
				while (reader.Read())
				{
					returnItem.Id = reader.GetInt32(0);
					returnItem.Value = reader.GetInt32(1);
					returnItem.Time = TimeSpan.FromSeconds(reader.GetInt32(2));
				}
			}

			return returnItem;
		}

	}
}
