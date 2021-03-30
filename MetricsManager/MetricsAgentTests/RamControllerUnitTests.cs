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
	public class RamControllerUnitTests
	{
		private RamMetricsController controller;
		private Mock<ILogger<RamMetricsController>> mockLogger;
		private Mock<IRamMetricsRepository> mockRepository;

		public RamControllerUnitTests()
		{
			mockLogger = new Mock<ILogger<RamMetricsController>>();
			mockRepository = new Mock<IRamMetricsRepository>();
			controller = new RamMetricsController(mockLogger.Object, mockRepository.Object);
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