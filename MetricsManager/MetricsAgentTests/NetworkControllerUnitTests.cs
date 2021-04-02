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
using static MetricsAgent.Responses.NetworkMetricsResponses;

namespace MetricsAgentsTests
{
	public class NetworkControllerUnitTests
	{
		private NetworkMetricsController controller;
		private Mock<ILogger<NetworkMetricsController>> mockLogger;
		private Mock<INetworkMetricsRepository> mockRepository;

		public NetworkControllerUnitTests()
		{
			mockLogger = new Mock<ILogger<NetworkMetricsController>>();
			mockRepository = new Mock<INetworkMetricsRepository>();
			controller = new NetworkMetricsController(mockLogger.Object, mockRepository.Object);
		}

		[Fact]
		public void GetMetrics_ReturnsOk()
		{
			//Arrange
			var fromTime = DateTimeOffset.MinValue;
			var toTime = DateTimeOffset.Now;

			//фейковые метрики возвращаемые репозиторием
			var mockMetrics = new List<NetworkMetric>()
			{
				{ new NetworkMetric() { Id = 1, Time = TimeSpan.FromDays(5), Value = 100 } },
				{ new NetworkMetric() { Id = 2, Time = TimeSpan.FromDays(10), Value = 100 } }
			};
			mockRepository.
				Setup(repository => repository.GetByTimeInterval(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).
				Returns(mockMetrics);

			//Act
			var result = controller.GetMetrics(fromTime, toTime);

			var response = ((result as OkObjectResult).Value as AllNetworkMetricsResponse).Metrics;

			//сравнение полученных значений с ожидаемыми значениями
			bool check = true;
			if (mockMetrics.Count == response.Count)
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



	}
}