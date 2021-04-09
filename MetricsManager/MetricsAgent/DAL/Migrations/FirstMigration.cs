using FluentMigrator;
using System;
using System.Collections.Generic;

namespace MetricsAgent.DAL.Migrations
{
	// имена таблиц
	[Migration(1)]
	public class FirstMigration : Migration
	{
		private readonly List<string> tablesNames = new List<string>
			{
				"cpumetrics",
				"dotnetmetrics",
				"hddmetrics",
				"networkmetrics",
				"rammetrics",
			};

		public override void Up()
		{
			foreach(string tableName in tablesNames)
			{
				Create.Table(tableName)
					.WithColumn("Id").AsInt64().PrimaryKey().Identity()
					.WithColumn("Value").AsInt32()
					.WithColumn("Time").AsInt64();
			}

			// Заполняем базу данных мусором для тестов
			foreach (string name in tablesNames)
			{
				for (int i = 0; i < 10; i++)
				{
					DateTimeOffset time = new DateTime(2000 + i, 1, 1);
					Insert.IntoTable(name)
						.Row(new { Value = i * 10 + tablesNames.IndexOf(name), Time = time.ToUnixTimeSeconds() });
				}
			}

		}

		public override void Down()
		{
			foreach (string tableName in tablesNames)
			{
				Delete.Table(tableName);
			}

		}
	}
}