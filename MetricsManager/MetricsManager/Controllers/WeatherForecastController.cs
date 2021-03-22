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

		#region ---- METHODS ----

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
				var item = from weatherForecast in _holder
							where weatherForecast.Date == date.Value
							select weatherForecast;

				if(item.Any() != true)
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


		/// <summary>
		/// Выдает значения температуры, за определенный интервал дат
		/// </summary>
		/// <param name="dateFrom">Начальная дата</param>
		/// <param name="dateTo">Конечная дата</param>
		/// <returns>Список со значениями температур за указанный период</returns>
		[HttpGet("read")]
		public IActionResult Read([FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
		{
			if (!dateFrom.HasValue)
			{
				dateFrom = DateTime.MinValue;
			}
			if(!dateTo.HasValue)
			{
				dateTo = DateTime.MaxValue;
			}

			var items = from weatherForecast in _holder
						where weatherForecast.Date >= dateFrom.Value && weatherForecast.Date <= dateTo.Value
						select weatherForecast;

			return Ok(items);
		}


		/// <summary>
		/// Изменяет значение температуры в заданную дату
		/// </summary>
		/// <param name="date">Дата за которую нужно изменить значение температуры</param>
		/// <param name="temperature">Новое значение температуры</param>
		/// <returns></returns>
		[HttpPut("update")]
		public IActionResult Update([FromQuery] DateTime? date, [FromQuery] int? temperature)
		{
			if (date.HasValue && temperature.HasValue)
			{
				try
				{
					var updatedWeatherForecast = _holder.Single(weatherForecast => weatherForecast.Date == date);
					updatedWeatherForecast.TemperatureC = temperature.Value;
				}
				catch
				{
					return BadRequest();
				}
			}
			else
			{
				return BadRequest();
			}
			return Ok();
		}


		/// <summary>
		/// Удаляет из списка показаний температур значение за указанный интервал дат
		/// </summary>
		/// <param name="dateFrom">Начальная дата</param>
		/// <param name="dateTo">Конечная дата</param>
		/// <returns></returns>
		[HttpDelete("delete")]
		public IActionResult Delete([FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
		{
			if (dateFrom.HasValue && dateTo.HasValue)
			{
				var items = from weatherForecast in _holder
							where weatherForecast.Date >= dateFrom.Value && weatherForecast.Date <= dateTo.Value
							select weatherForecast;

				//ToList, для того чтобы прошло немедленное выполнение запроса, иначе выскакивает exception при удалении элементов
				foreach (var item in items.ToList())
				{
					_holder.Remove(item);
				}
			}
			else
			{
				return BadRequest();
			}

			return Ok();
		}

		#endregion
	}
}
