using AutoMapper;
using MetricsManager.DAL;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using MetricsManager.Requests;
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

			var response = new AllAgentsInfoResponse();

			foreach (var agentInfo in allAgentsInfo.Agents)
			{
				response.Agents.Add(_mapper.Map<AgentInfoDto>(agentInfo));
			}

			return Ok(response);
		}

		[HttpPost("register")]
		public IActionResult RegisterAgent([FromBody] AgentInfoRegisterRequest request)
		{
			_logger.LogDebug("Вызов метода. Параметры:" +
				$" {nameof(request.AgentId)} = {request.AgentId}" +
				$" {nameof(request.AgentUri)} = {request.AgentUri}");

			var agentInfo = new AgentInfo() { AgentId = request.AgentId, AgentUri = request.AgentUri};

			_repository.RegisterAgent(agentInfo);

			return Ok();
		}

	}
	
}

