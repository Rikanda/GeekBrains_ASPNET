using Metrics.Tools;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MetricsAgentsTests
{
	public class NetworkControllerUnitTests
	{
		private NetworkMetricsController controller;
		private Mock<ILogger<NetworkMetricsController>> mockLogger;

		public NetworkControllerUnitTests()
		{
			mockLogger = new Mock<ILogger<NetworkMetricsController>>();
			controller = new NetworkMetricsController(mockLogger.Object);
		}

		[Fact]
		public void GetMetricsFromAgent_ReturnsOk()
		{
			//Arrange
			var fromTime = TimeSpan.FromSeconds(0);
			var toTime = TimeSpan.FromSeconds(100);

			//Act
			var result = controller.GetMetricsFromAgent(fromTime, toTime);

			// Assert
			_ = Assert.IsAssignableFrom<IActionResult>(result);
		}



	}
}