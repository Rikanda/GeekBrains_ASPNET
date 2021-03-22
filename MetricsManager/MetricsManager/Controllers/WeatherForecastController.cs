using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MetricsManager.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WeatherForecastController : ControllerBase
	{
		#region ---- CONSTANTS ----

		/// <summary>Формат даты для парсинга запросов</summary>
		private readonly string _format = "yyyy-MM-dd";

		#endregion

		#region ---- FIELDS ----

		/// <summary>Список для хранения значений температуры</summary>
		private readonly List<WeatherForecast> _holder;

		#endregion

		#region ---- CONSTRUCTORS ----

		public WeatherForecastController(List<WeatherForecast> holder)
		{
			this._holder = holder;
		}

		#endregion

		#region ---- CREATE ----

		/// <summary>
		/// Создает запись в списке с показаниями температуры
		/// </summary>
		/// <param name="date">Дата</param>
		/// <param name="temperature">Значение температуры в градусах Цельсия</param>
		/// <returns></returns>
		[HttpPost("create")]
		public IActionResult Create([FromQuery] DateTime? date, [FromQuery] int? temperature)
		{
			if(date.HasValue && temperature.HasValue)
			{
				var value = from weatherForecast in _holder
							where weatherForecast.Date == date.Value
							select weatherForecast;

				if(value.Any() != true)
				{
					_holder.Add(new WeatherForecast(date.Value, temperature.Value));
				}
			}
			else
			{
				return BadRequest();
			}

			return Ok();
		}

		#endregion

		#region ---- READ ----

		/// <summary>
		/// Выдает значения температуры, за определенный интервал дат
		/// </summary>
		/// <param name="dateFrom">Начальная дата</param>
		/// <param name="dateTo">Конечная дата</param>
		/// <returns>Список со значениями температур за указанный период</returns>
		[HttpGet("readInterval")]
		public IActionResult Read([FromQuery] string dateFrom, [FromQuery] string dateTo)
		{
			bool isProceed = false;
			DateTime dateFromParsed = DateTime.MinValue;
			DateTime dateToParsed = DateTime.MinValue;

			isProceed = DateTime.TryParseExact(dateFrom, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateFromParsed);
			if (isProceed) isProceed = DateTime.TryParseExact(dateTo, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateToParsed);

			if (isProceed)
			{
				var value = from weatherForecast in _holder
							where weatherForecast.Date >= dateFromParsed && weatherForecast.Date <= dateToParsed
							select weatherForecast;
				return Ok(value);
			}

			return Ok(isProceed);
		}

		#endregion

		#region ---- UPDATE ----

		/// <summary>
		/// 
		/// </summary>
		/// <param name="date"></param>
		/// <param name="temperature"></param>
		/// <returns></returns>
		[HttpPut("update")]
		public IActionResult Update([FromQuery] string date, [FromQuery] string temperature)
		{
			bool isProceed = false;
			int temperatureParsed = 0;
			DateTime dateParsed = DateTime.MinValue;

			isProceed = DateTime.TryParseExact(date, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateParsed);
			if (isProceed) isProceed = int.TryParse(temperature, out temperatureParsed);


			if (isProceed)
			{
				WeatherForecast newItem = new WeatherForecast(dateParsed, temperatureParsed);

				var value = from weatherForecast in _holder
							where weatherForecast.Date == dateParsed
							select weatherForecast;

				WeatherForecast[] oldItems = value.ToArray<WeatherForecast>();

				if (oldItems.Length != 0)
				{
					_holder.Remove(oldItems[0]);
					_holder.Add(newItem);
				}
				else
				{
					isProceed = false;
				}
			}

			return Ok(isProceed);
		}

		#endregion

		#region ---- DELETE ----

		/// <summary>
		/// Удаляет из списка показаний температур значение за указанную дату
		/// </summary>
		/// <param name="date">Дата за которую нужно удалить значение температуры</param>
		/// <returns>true, если процесс удаления прошел успешно</returns>
		[HttpDelete("delete")]
		public IActionResult Delete([FromQuery] string date)
		{
			bool isProceed = false;
			DateTime dateParsed = DateTime.MinValue;

			isProceed = DateTime.TryParseExact(date, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateParsed);

			if (isProceed)
			{
				var value = from i in _holder
							where i.Date == dateParsed
							select i;

				WeatherForecast[] oldItems = value.ToArray<WeatherForecast>();

				if (oldItems.Length != 0)
				{
					_holder.Remove(oldItems[0]);
				}
				else
				{
					isProceed = false;
				}
			}

			return Ok(isProceed);
		}

		#endregion
	}
}
