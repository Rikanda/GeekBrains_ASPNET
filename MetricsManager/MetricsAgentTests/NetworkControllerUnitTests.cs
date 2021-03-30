using Metrics.Tools;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
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
		private Mock<INetworkMetricsRepository> mockRepository;

		public NetworkControllerUnitTests()
		{
			mockLogger = new Mock<ILogger<NetworkMetricsController>>();
			mockRepository = new Mock<INetworkMetricsRepository>();
			controller = new NetworkMetricsController(mockLogger.Object, mockRepository.Object);
		}

		[Fact]
		public void GetMetrics_ReturnsOk()
		{
			//Arrange
			var fromTime = DateTimeOffset.MinValue;
			var toTime = DateTimeOffset.Now;

			//Act
			var result = controller.GetMetrics(fromTime, toTime);

			// Assert
			_ = Assert.IsAssignableFrom<IActionResult>(result);
		}



	}
}