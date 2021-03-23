using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AgentsController : ControllerBase
	{
		//!DELETE Отладочный метод для проверки сервера
		[HttpGet("read")]
		public IActionResult Read()
		{
			return Ok("Запуск MetricsManager прошел успешно");
		}

		[HttpPost("register")]
		public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
		{
			return Ok();
		}

		[HttpPut("enable/{agentId}")]
		public IActionResult EnableAgentById([FromRoute] int agentId)
		{
			return Ok();
		}

		[HttpPut("disable/{agentId}")]
		public IActionResult DisableAgentById([FromRoute] int agentId)
		{
			return Ok();
		}
	}
	
	public class AgentInfo
	{
		public int AgentId { get; set; }
		public Uri AgentAddress { get; set; }
	}
}
