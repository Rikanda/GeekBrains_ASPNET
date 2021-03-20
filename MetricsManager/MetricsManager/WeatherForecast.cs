using System;

namespace MetricsManager
{
	/// <summary>
	/// Хранит значение температуры в определенный день
	/// </summary>
	public class WeatherForecast
	{
		#region ---- PROPERTIES ----

		/// <summary>Дата</summary>
		public DateTime Date { get; set; }

		/// <summary>Значение температуры в градусах цельсия</summary>
		public int TemperatureC { get; set; }

		#endregion

		#region ---- CONSTRUTORS ----

		/// <summary>Создает новый элемент</summary>
		/// <param name="date">Дата</param>
		/// <param name="temperature">Температрура в градусах цельсия</param>
		public WeatherForecast(DateTime date, int temperature)
		{
			Date = date;
			TemperatureC = temperature;
		}

		#endregion
	}
}
