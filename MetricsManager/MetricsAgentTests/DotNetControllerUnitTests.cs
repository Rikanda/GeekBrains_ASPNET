using Metrics.Tools;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MetricsAgentsTests
{
	public class DotNetControllerUnitTests
	{
		private DotNetMetricsController controller;
		private Mock<ILogger<DotNetMetricsController>> mockLogger;

		public DotNetControllerUnitTests()
		{
			mockLogger = new Mock<ILogger<DotNetMetricsController>>();
			controller = new DotNetMetricsController(mockLogger.Object);
		}

		[Fact]
		public void GetMetrics_ReturnsOk()
		{
			//Arrange
			var fromTime = TimeSpan.FromSeconds(0);
			var toTime = TimeSpan.FromSeconds(100);

			//Act
			var result = controller.GetMetrics(fromTime, toTime);

			// Assert
			_ = Assert.IsAssignableFrom<IActionResult>(result);
		}



	}
}