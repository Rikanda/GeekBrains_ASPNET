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
	public class DotNetControllerUnitTests
	{
		private DotNetMetricsController controller;
		private Mock<ILogger<DotNetMetricsController>> mockLogger;
		private Mock<IDotNetMetricsRepository> mockRepository;

		public DotNetControllerUnitTests()
		{
			mockLogger = new Mock<ILogger<DotNetMetricsController>>();
			mockRepository = new Mock<IDotNetMetricsRepository>();
			controller = new DotNetMetricsController(mockLogger.Object, mockRepository.Object);
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