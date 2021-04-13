using Metrics.Tools;
using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MetricsManagerTests
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
		public void GetMetricsFromAgent_ReturnsOk()
		{
			//Arrange
			var agentId = 1;
			var fromTime = DateTimeOffset.MinValue;
			var toTime = DateTimeOffset.UtcNow;

			//Act
			var result = controller.GetMetricsFromAgent(agentId, fromTime, toTime);

			// Assert
			_ = Assert.IsAssignableFrom<IActionResult>(result);
		}

		[Fact]
		public void GetMetricsByPercentileFromAgent_ReturnsOk()
		{
			//Arrange
			var agentId = 1;
			var fromTime = DateTimeOffset.MinValue;
			var toTime = DateTimeOffset.UtcNow;
			var percentile = Percentile.P90;

			//Act
			var result = controller.GetMetricsByPercentileFromAgent(agentId, fromTime, toTime, percentile);

			// Assert
			_ = Assert.IsAssignableFrom<IActionResult>(result);
		}

		[Fact]
		public void GetMetricsFromAllCluster_ReturnsOk()
		{
			//Arrange
			var fromTime = DateTimeOffset.MinValue;
			var toTime = DateTimeOffset.UtcNow;

			//Act
			var result = controller.GetMetricsFromAllCluster(fromTime, toTime);

			// Assert
			_ = Assert.IsAssignableFrom<IActionResult>(result);
		}

		[Fact]
		public void GetMetricsByPercentileFromAllCluster_ReturnsOk()
		{
			//Arrange
			var fromTime = DateTimeOffset.MinValue;
			var toTime = DateTimeOffset.UtcNow;
			var percentile = Percentile.P90;

			//Act
			var result = controller.GetMetricsByPercentileFromAllCluster(fromTime, toTime, percentile);

			// Assert
			_ = Assert.IsAssignableFrom<IActionResult>(result);
		}
	}
}
