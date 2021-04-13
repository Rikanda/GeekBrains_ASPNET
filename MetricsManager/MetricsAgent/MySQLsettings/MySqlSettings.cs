using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.SQLsettings
{
	/// <summary> Ключи имен таблиц </summary>
	public enum Tables
	{
		CpuMetric,
		DotNetMetric,
		HddMetric,
		NetworkMetric,
		RamMetric,
	}

	/// <summary> Ключи имен рядов </summary>
	public enum Columns
	{
		Id,
		Value,
		Time,
	}

	/// <summary>Класс для хранения настроек базы данных</summary>
	public class MySqlSettings : IMySqlSettings
	{
		/// <summary>Словарь для хранения имен таблиц</summary>
		private Dictionary<Tables, string> tablesNames = new Dictionary<Tables, string>
		{
			{Tables.CpuMetric, "cpumetric" },
			{Tables.DotNetMetric, "dotnetmetric" },
			{Tables.HddMetric, "hddmetric" },
			{Tables.NetworkMetric, "networkmetric" },
			{Tables.RamMetric, "rammetric" },
		};


		/// <summary>Словарь для хранения имен рядов в таблицах</summary>
		private Dictionary<Columns, string> rowsNames = new Dictionary<Columns, string>
		{
			{Columns.Id, "id" },
			{Columns.Value, "value" },
			{Columns.Time, "time" },
		};

		/// <summary> Строка для подключения к базе данных </summary>
		private readonly string connectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;journal mode=Wal;";


		public string ConnectionString
		{
			get
			{
				return connectionString;
			}
		}

		public string this[Tables key]
		{
			get
			{
				return tablesNames[key];
			}
		}

		public string this[Columns key]
		{
			get
			{
				return rowsNames[key];
			}
		}

	}
}
