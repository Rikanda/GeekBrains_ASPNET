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
		public IActionResult Create([FromQuery] string date, [FromQuery] string temperature)
		{
			bool isProceed;
			int temperatureParsed = 0;

			isProceed = DateTime.TryParseExact(date, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateParsed);
			if (isProceed) isProceed = int.TryParse(temperature, out temperatureParsed);

			if (isProceed)
			{
				WeatherForecast newItem = new WeatherForecast(dateParsed, temperatureParsed);
				if (!_holder.Contains(newItem))
				{
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

		#region ---- READ ----

		/// <summary>
		/// Просмотр всех сохраненных значений температуры
		/// </summary>
		/// <returns>Список со всеми значениями температуры</returns>
		[HttpGet("read")]
		public IActionResult Read()
		{
			return Ok(_holder);
		}

		/// <summary>Выдает значение температуры за определенную дату</summary>
		/// <param name="date">Дата за которую нужно узнать значение температуры</param>
		/// <returns>Запись из списка значений температур</returns>
		[HttpGet("readValue")]
		public IActionResult Read([FromQuery] string date)
		{
			bool isProceed = false;
			DateTime dateParsed = DateTime.MinValue;

			isProceed = DateTime.TryParseExact(date, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateParsed);

			if (isProceed)
			{
				var value = from i in _holder
							where i.Date == dateParsed
							select i;
				return Ok(value);
			}

			return Ok(isProceed);
		}

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
				var value = from i in _holder
							where i.Date >= dateFromParsed && i.Date <= dateToParsed
							select i;
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

				var value = from i in _holder
							where i.Date == dateParsed
							select i;

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
		/// Удаляет все записи из списка показаний температур
		/// </summary>
		/// <returns></returns>
		[HttpDelete("deleteAll")]
		public IActionResult Delete()
		{
			_holder.Clear();
			return Ok();
		}


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
