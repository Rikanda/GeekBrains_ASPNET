using Metrics.Tools;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static MetricsAgent.Responses.DotNetMetricsResponses;

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
			var fromTime = DateTimeOffset.MinValue;
			var toTime = DateTimeOffset.Now;

			//фейковые метрики возвращаемые репозиторием
			var mockMetrics = new List<CpuMetric>()
			{
				{ new CpuMetric() { Id = 1, Time = TimeSpan.FromDays(5), Value = 100 } },
				{ new CpuMetric() { Id = 2, Time = TimeSpan.FromDays(10), Value = 100 } }
			};

			mockRepository.
				Setup(repository => repository.GetByTimeInterval(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).
				Returns(mockMetrics);

			//Act
			var result = controller.GetMetrics(fromTime, toTime);

			var response = ((result as OkObjectResult).Value as AllCpuMetricsResponse).Metrics;

			//сравнение полученных значений с ожидаемыми значениями
			bool check = true;
			if(mockMetrics.Count == response.Count)
			{
				for (int i = 0; i < mockMetrics.Count; i++)
				{
					if ((mockMetrics[i].Id != response[i].Id) ||
						(mockMetrics[i].Value != response[i].Value) ||
						(mockMetrics[i].Time != response[i].Time))
					{
						check = false;
					}
				}
			}
			else
			{
				check = false;
			}

			// Assert
			Assert.True(check);
		}

		[Fact]
		public void GetMetricsByPercentile_ReturnsOk()
		{
			//Arrange
			var fromTime = DateTimeOffset.MinValue;
			var toTime = DateTimeOffset.Now;
			var percentile = Percentile.P90;

			//Act
			var result = controller.GetMetricsByPercentile(fromTime, toTime, percentile);

			// Assert
			_ = Assert.IsAssignableFrom<IActionResult>(result);
		}

	}
}