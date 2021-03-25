using Metrics.Tools;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentsTests
{
    public class CpuControllerUnitTests
    {
        private CpuMetricsController controller;
        public CpuControllerUnitTests()
        {
            controller = new CpuMetricsController();
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

        [Fact]
        public void GetMetricsByPercentileFromAgent_ReturnsOk()
        {
            //Arrange
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            var percentile = Percentile.P90;

            //Act
            var result = controller.GetMetricsByPercentileFromAgent(fromTime, toTime, percentile);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

    }
}