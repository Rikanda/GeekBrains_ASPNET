using Dapper;
using System.Data;
using System;

namespace MetricsManager.DAL
{
	/// <summary>
	/// Хэндлер для парсинга значений в DateTimeOffset если таковые попадаются в классах моделей
	/// </summary>
	public class DateTimeOffsetHandler : SqlMapper.TypeHandler<DateTimeOffset>
	{
		public override DateTimeOffset Parse(object value)
			=> DateTimeOffset.FromUnixTimeSeconds((long)value);

		public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
			=> parameter.Value = value;
	}
}