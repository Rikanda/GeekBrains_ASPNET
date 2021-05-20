using Dapper;
using FluentMigrator;
using MetricsManager.MySQLsettings;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MetricsManager.DAL.Migrations
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
			Create.Table(mySql.AgentsTable)
				.WithColumn(mySql[Columns.Id]).AsInt64().PrimaryKey().Identity()
				.WithColumn(mySql[Columns.AgentId]).AsInt32()
				.WithColumn(mySql[Columns.AgentUri]).AsString();

			foreach (Tables tableName in Enum.GetValues(typeof(Tables)))
			{
				Create.Table(mySql[tableName])
					.WithColumn(mySql[Columns.Id]).AsInt64().PrimaryKey().Identity()
					.WithColumn(mySql[Columns.AgentId]).AsInt32()
					.WithColumn(mySql[Columns.Value]).AsInt32()
					.WithColumn(mySql[Columns.Time]).AsInt64();
			}

			//! DEBUG Тестовые данные по агентам
			Insert.IntoTable(mySql.AgentsTable)
				.Row(new { AgentId = 1, AgentUri = "http://metricsagent.verm-v.ru" });
		}

		public override void Down()
		{
			foreach (Tables tableName in Enum.GetValues(typeof(Tables)))
			{
				Delete.Table(mySql[tableName]);
			}
			
			Delete.Table(mySql.AgentsTable);
		}
	}
}