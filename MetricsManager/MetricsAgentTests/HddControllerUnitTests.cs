using Metrics.Tools;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MetricsAgentsTests
{
	public class HddControllerUnitTests
	{
		private HddMetricsController controller;
		private Mock<ILogger<HddMetricsController>> mockLogger;

		public HddControllerUnitTests()
		{
			mockLogger = new Mock<ILogger<HddMetricsController>>();
			controller = new HddMetricsController(mockLogger.Object);
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