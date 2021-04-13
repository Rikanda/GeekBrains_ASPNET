using Dapper;
using MetricsManager.SQLsettings;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL
{
	public class AgentsRepository : IAgentsRepository
	{
		/// <summary>
		/// Объект с именами и настройками базы данных
		/// </summary>
		private readonly IMySqlSettings mySql;

		public AgentsRepository(IMySqlSettings mySqlSettings)
		{
			mySql = mySqlSettings;
		}

		public AgentInfo GetAgentInfoById(int agentId)
		{
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				return connection.QuerySingle<AgentInfo>(
				"SELECT * " +
				$"FROM {mySql.AgentsTable} " +
				$"WHERE (" +
				$"{mySql[Columns.AgentId]} == @agentId)",
				new
				{
					agentId = agentId
				});
			}

		}

		public IList<AgentInfo> GetAllAgentsInfo()
		{
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				return connection.Query<AgentInfo>(
				"SELECT * " +
				$"FROM {mySql.AgentsTable} ").ToList();
			}
		}

		public void RegisterAgent(AgentInfo agentInfo)
		{
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				connection.ExecuteAsync(
				$"INSERT INTO {mySql.AgentsTable}" +
				$"({mySql[Columns.AgentId]}, {mySql[Columns.AgentUri]})" +
				$"VALUES (@agentId, @agentUri);",
				new
				{
					agentId = agentInfo.AgentId,
					agentUri = agentInfo.AgentUri,
				});
			}
		}
	}
}
