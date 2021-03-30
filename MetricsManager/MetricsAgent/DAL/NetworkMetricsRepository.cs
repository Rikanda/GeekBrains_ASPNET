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
	public interface INetworkMetricsRepository : IRepository<NetworkMetric>
	{
	}

	public class NetworkMetricsRepository : INetworkMetricsRepository
	{
		// наше соединение с базой данных
		private readonly SQLiteConnection connection;

		// инжектируем соединение с базой данных в наш репозиторий через конструктор
		public NetworkMetricsRepository(SQLiteConnection connection)
		{
			this.connection = connection;
		}

		public IList<NetworkMetric> GetByTimeInterval(DateTimeOffset fromTime, DateTimeOffset toTime)
		{
			using var cmd = new SQLiteCommand(connection);

			// прописываем в команду SQL запрос на получение всех данных из таблицы
			cmd.CommandText = "SELECT * FROM networkmetrics WHERE (time >= @fromtime AND time <= @totime)";
			cmd.Parameters.AddWithValue("@fromtime", fromTime.ToUnixTimeSeconds());
			cmd.Parameters.AddWithValue("@totime", toTime.ToUnixTimeSeconds());

			var returnList = new List<NetworkMetric>();

			using (SQLiteDataReader reader = cmd.ExecuteReader())
			{
				// пока есть что читать -- читаем
				while (reader.Read())
				{
					// добавляем объект в список возврата
					returnList.Add(new NetworkMetric
					{
						Id = reader.GetInt32(0),
						Value = reader.GetInt32(1),
						Time = TimeSpan.FromSeconds(reader.GetInt32(2))
					});
				}
			}

			return returnList;
		}

		public NetworkMetric GetLast()
		{
			using var cmd = new SQLiteCommand(connection);

			// прописываем в команду SQL запрос на получение всех данных из таблицы
			cmd.CommandText = "SELECT * FROM networkmetrics WHERE (time = (SELECT MAX(time) FROM networkmetrics))";

			var returnItem = new NetworkMetric();

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
