using Metrics.Tools;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MetricsAgentsTests
{
	public class RamControllerUnitTests
	{
		private RamMetricsController controller;
		private Mock<ILogger<RamMetricsController>> mockLogger;

		public RamControllerUnitTests()
		{
			mockLogger = new Mock<ILogger<RamMetricsController>>();
			controller = new RamMetricsController(mockLogger.Object);
		}

		[Fact]
		public void GetMetrics_ReturnsOk()
		{
			//Arrange

			//Act
			var result = controller.GetMetrics();

			// Assert
			_ = Assert.IsAssignableFrom<IActionResult>(result);
		}



	}
}