using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Requests
{
	/// <summary>
	/// Контейнер с данными для регистрации агента
	/// </summary>
	public class AgentInfoRegisterRequest
	{
		public int AgentId { get; set; }

		public string AgentUri { get; set; }

	}
}
