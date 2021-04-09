using Dapper;
using System.Data;
using System;

namespace MetricsAgent.DAL
{
	/// <summary>
	/// Хэндлер для парсинга значений в TimeSpan если таковые попадаются в классах моделей
	/// </summary>
	public class TimeSpanHandler : SqlMapper.TypeHandler<TimeSpan>
	{
		public override TimeSpan Parse(object value)
			=> TimeSpan.FromSeconds((long)value);

		public override void SetValue(IDbDataParameter parameter, TimeSpan value)
			=> parameter.Value = value;
	}
}