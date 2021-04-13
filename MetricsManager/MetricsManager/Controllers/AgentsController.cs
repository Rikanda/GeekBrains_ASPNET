using AutoMapper;
using MetricsManager.DAL;
using MetricsManager.Responses;
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

	[Route("api/agents")]
	[ApiController]
	public class AgentsController : ControllerBase
	{
		private readonly ILogger<AgentsController> _logger;
		private readonly IAgentsRepository _repository;
		private readonly IMapper _mapper;

		public AgentsController(ILogger<AgentsController> logger, IAgentsRepository repository, IMapper mapper)
		{
			_logger = logger;
			_logger.LogDebug("Вызов конструктора");
			_repository = repository;
			_mapper = mapper;
		}

		[HttpGet("read")]
		public IActionResult Read()
		{
			_logger.LogDebug("Вызов метода");

			var allAgentsInfo = _repository.GetAllAgentsInfo();

			var response = new AllAgentsInfoResponse()
			{
				Agents = new List<AgentInfoDto>()
			};

			foreach (var agentInfo in allAgentsInfo)
			{
				response.Agents.Add(_mapper.Map<AgentInfoDto>(agentInfo));
			}

			return Ok(response);
		}

		[HttpPost("register")]
		public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(agentInfo.AgentId)} = {agentInfo.AgentId}" +
				$" {nameof(agentInfo.AgentUri)} = {agentInfo.AgentUri}");

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
	
}

