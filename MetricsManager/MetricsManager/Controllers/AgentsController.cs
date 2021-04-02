using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class AgentsController : ControllerBase
	{
		private readonly ILogger<AgentsController> _logger;

		public AgentsController(ILogger<AgentsController> logger)
		{
			_logger = logger;
			_logger.LogDebug("Вызов конструктора");
		}

		[HttpGet("read")]
		public IActionResult Read()
		{
			_logger.LogDebug("Вызов метода");
			return Ok("Список зарегистрированных в системе агентов");
		}

		[HttpPost("register")]
		public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(agentInfo.AgentId)} = {agentInfo.AgentId}" +
				$" {nameof(agentInfo.AgentAddress)} = {agentInfo.AgentAddress}");

			return Ok();
		}

		[HttpPut("enable/{agentId}")]
		public IActionResult EnableAgentById([FromRoute] int agentId)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(agentId)} = {agentId}");

			return Ok();
		}

		[HttpPut("disable/{agentId}")]
		public IActionResult DisableAgentById([FromRoute] int agentId)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(agentId)} = {agentId}");

			return Ok();
		}
	}
	
	public class AgentInfo
	{
		public int AgentId { get; set; }
		public Uri AgentAddress { get; set; }
	}
}
