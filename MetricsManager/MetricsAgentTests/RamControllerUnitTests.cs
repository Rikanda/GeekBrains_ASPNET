using Metrics.Tools;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;
using static MetricsAgent.Responses.RamMetricsResponses;

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
			//фейковая метрика возвращаемая репозиторием
			var mockMetric = new RamMetric() { Id = 1, Time = TimeSpan.Zero, Value = 100 };
			mockRepository.
				Setup(repository => repository.GetLast()).
				Returns(mockMetric);

			//Act
			var result = controller.GetMetrics();

			var response = (result as OkObjectResult).Value as RamMetricDto;

			//сравнение полученных значений с ожидаемыми значениями
			bool check = true;
			if (response != null)
			{
				if ((mockMetric.Id != response.Id) ||
					(mockMetric.Value != response.Value) ||
					(mockMetric.Time != response.Time))
				{
					check = false;
				}
			}
			else
			{
				check = false;
			}

			// Assert
			Assert.True(check);
		}



	}
}