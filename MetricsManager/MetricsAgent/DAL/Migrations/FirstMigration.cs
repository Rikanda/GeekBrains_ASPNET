using FluentMigrator;
using MetricsAgent.SQLsettings;
using System;
using System.Collections.Generic;

namespace MetricsAgent.DAL.Migrations
{
	// имена таблиц
	[Migration(1)]
	public class FirstMigration : Migration
	{
		/// <summary>
		/// Объект с именами и настройками базы данных
		/// </summary>
		private readonly IMySqlSettings mySql;

		public FirstMigration(IMySqlSettings mySqlSettings)
		{
			mySql = mySqlSettings;
		}

		public override void Up()
		{
			foreach(Tables tableName in Enum.GetValues(typeof(Tables)))
			{
				Create.Table(mySql[tableName])
					.WithColumn(mySql[Rows.Id]).AsInt64().PrimaryKey().Identity()
					.WithColumn(mySql[Rows.Value]).AsInt32()
					.WithColumn(mySql[Rows.Time]).AsInt64();
			}
		}

		public override void Down()
		{
			foreach (Tables tableName in Enum.GetValues(typeof(Tables)))
			{
				Delete.Table(mySql[tableName]);
			}

		}
	}
}