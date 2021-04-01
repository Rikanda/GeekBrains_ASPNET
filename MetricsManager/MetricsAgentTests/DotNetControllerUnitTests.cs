using Metrics.Tools;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using static MetricsAgent.Responses.DotNetMetricsResponses;

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

			//фейковая метрика возвращаемая репозиторием
			var mockMetrics = new List<DotNetMetric>() { { new DotNetMetric() { Id = 1, Time = TimeSpan.Zero, Value = 100 } } };
			mockRepository.
				Setup(repository => repository.GetByTimeInterval(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).
				Returns(mockMetrics); ;

			//Act
			var result = controller.GetMetrics(fromTime, toTime);

			var response = (result as OkObjectResult).Value as AllDotNetMetricsResponse;

			// Assert
			Assert.True(response.Metrics.Count != 0);
		}



	}
}