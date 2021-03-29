using Metrics.Tools;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MetricsAgentsTests
{
	public class CpuControllerUnitTests
	{
		private CpuMetricsController controller;
		private Mock<ILogger<CpuMetricsController>> mockLogger;
		private Mock<ICpuMetricsRepository> mockRepository;

		public CpuControllerUnitTests()
		{
			mockLogger = new Mock<ILogger<CpuMetricsController>>();
			mockRepository = new Mock<ICpuMetricsRepository>();
			controller = new CpuMetricsController(mockLogger.Object, mockRepository.Object);
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

		[Fact]
		public void GetMetricsByPercentile_ReturnsOk()
		{
			//Arrange
			var fromTime = TimeSpan.FromSeconds(0);
			var toTime = TimeSpan.FromSeconds(100);
			var percentile = Percentile.P90;

			//Act
			var result = controller.GetMetricsByPercentile(fromTime, toTime, percentile);

			// Assert
			_ = Assert.IsAssignableFrom<IActionResult>(result);
		}

	}
}