using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManagerClient.Responses.FromManager
{
	/// <summary>
	/// Контейнер для передачи списка с информацие об агентах
	/// </summary>
	public class AllAgentsInfoResponse
	{
		public List<AgentInfoDto> Agents { get; set; }

		public AllAgentsInfoResponse()
		{
			Agents = new List<AgentInfoDto>();
		}

	}

	/// <summary>
	/// Контейнер для передачи информации об агенте
	/// </summary>
	public class AgentInfoDto
	{
		public int AgentId { get; set; }
		public string AgentUri { get; set; }
	}
}
