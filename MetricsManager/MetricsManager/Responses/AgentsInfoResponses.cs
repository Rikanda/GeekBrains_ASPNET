using System;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
	/// <summary>
	/// Контейнер для передачи списка с информацие об агентах
	/// </summary>
	public class AllAgentsInfoResponse
	{
		public List<AgentInfoDto> Agents { get; set; }
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