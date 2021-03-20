using System;

namespace MetricsManager
{
	/// <summary>
	/// ������ �������� ����������� � ������������ ����
	/// </summary>
	public class WeatherForecast
	{
		#region ---- PROPERTIES ----

		/// <summary>����</summary>
		public DateTime Date { get; set; }

		/// <summary>�������� ����������� � �������� �������</summary>
		public int TemperatureC { get; set; }

		#endregion

		#region ---- CONSTRUTORS ----

		/// <summary>������� ����� �������</summary>
		/// <param name="date">����</param>
		/// <param name="temperature">������������ � �������� �������</param>
		public WeatherForecast(DateTime date, int temperature)
		{
			Date = date;
			TemperatureC = temperature;
		}

		#endregion
	}
}
