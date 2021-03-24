using Metrics.Tools;
using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class AgentsControllerUnitTests
    {
        private AgentsController controller;
        public AgentsControllerUnitTests()
        {
            controller = new AgentsController();
        }

        [Fact]
        public void Read_ReturnsOk()
        {
            //Arrange

            //Act
            var result = controller.Read();

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void RegistrAgent_ReturnsOk()
        {
            //Arrange
            var agentInfo = new AgentInfo() { AgentId = 101, AgentAddress = new Uri("http://AgentAdressUri") };

            //Act
            var result = controller.RegisterAgent(agentInfo);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void EnableAgentById_ReturnsOk()
        {
            //Arrange
            var agentId = 1;

            //Act
            var result = controller.EnableAgentById(agentId);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void DisableAgentById_ReturnsOk()
        {
            //Arrange
            var agentId = 1;

            //Act
            var result = controller.DisableAgentById(agentId);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }


    }
}