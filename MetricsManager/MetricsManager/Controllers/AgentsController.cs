	using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
		private readonly ILogger<AgentsController> _logger;

		public AgentsController(ILogger<AgentsController> logger)
		{
			_logger = logger;

			_logger.LogInformation("AgentsController class logging levels:");
			_logger.LogInformation($"Trace: {_logger.IsEnabled(LogLevel.Trace)}");
			_logger.LogInformation($"Debug: {_logger.IsEnabled(LogLevel.Debug)}");
			_logger.LogInformation($"Info: {_logger.IsEnabled(LogLevel.Information)}");
			_logger.LogInformation($"Warn: {_logger.IsEnabled(LogLevel.Warning)}");
			_logger.LogInformation($"Error: {_logger.IsEnabled(LogLevel.Error)}");
			_logger.LogInformation($"Fatal: {_logger.IsEnabled(LogLevel.Critical)}");

			_logger.LogInformation(" AgentsController class test samples");
			_logger.LogTrace("1 -- TRACE --");
			_logger.LogDebug("2 -- DEBUG --");
			_logger.LogInformation(" 3 -- INFO  --");
			_logger.LogWarning(" 4 -- WARN  --");
			_logger.LogError("5 -- ERROR --");
			_logger.LogCritical("6 -- FATAL --");
		}

		[HttpGet("read")]
		public IActionResult Read()
		{
			_logger.LogInformation("Вызван метод Read, AgentsController");

			return Ok("Список зарегистрированных в системе агентов");
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
