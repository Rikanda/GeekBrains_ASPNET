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
	public interface IHddMetricsRepository : IRepository<HddMetric>
	{
	}

	public class HddMetricsRepository : IHddMetricsRepository
	{
		// наше соединение с базой данных
		private SQLiteConnection connection;

		// инжектируем соединение с базой данных в наш репозиторий через конструктор
		public HddMetricsRepository(SQLiteConnection connection)
		{
			this.connection = connection;
		}

		public IList<HddMetric> GetByTimeInterval(TimeSpan fromTime, TimeSpan toTime)
		{
			using var cmd = new SQLiteCommand(connection);

			// прописываем в команду SQL запрос на получение всех данных из таблицы
			cmd.CommandText = "SELECT * FROM HddMetrics";

			var returnList = new List<HddMetric>();

			using (SQLiteDataReader reader = cmd.ExecuteReader())
			{
				// пока есть что читать -- читаем
				while (reader.Read())
				{
					// добавляем объект в список возврата
					returnList.Add(new HddMetric
					{
						Id = reader.GetInt32(0),
						Value = reader.GetInt32(1),
						// налету преобразуем прочитанные секунды в метку времени
						Time = TimeSpan.FromSeconds(reader.GetInt32(2))
					});
				}
			}

			return returnList;
		}
	}
}
