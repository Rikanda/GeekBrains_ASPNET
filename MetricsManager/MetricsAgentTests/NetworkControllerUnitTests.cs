using AutoMapper;
using MetricsAgent;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

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

			var myProfile = new MapperProfile();
			var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
			var mapper = new Mapper(configuration);

			controller = new NetworkMetricsController(mockLogger.Object, mockRepository.Object, mapper);
		}

		[Fact]
		public void GetMetricsByInterval_ReturnsCorrectMetrics()
		{
			//Arrange
			var request = new NetworkMetricGetByIntervalRequest()
			{
				fromTime = DateTimeOffset.MinValue,
				toTime = DateTimeOffset.Now
			};

			//фейковые метрики возвращаемые репозиторием
			var mockMetrics = new List<NetworkMetric>()
			{
				{ new NetworkMetric() { Time = TimeSpan.FromDays(5), Value = 100 } },
				{ new NetworkMetric() { Time = TimeSpan.FromDays(10), Value = 100 } }
			};
			mockRepository.
				Setup(repository => repository.GetByTimeInterval(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).
				Returns(mockMetrics);

			//Act
			var result = controller.GetMetrics(request);

			var response = ((result as OkObjectResult).Value as AllNetworkMetricsResponse).Metrics;

			//сравнение полученных значений с ожидаемыми значениями
			bool check = true;
			if (mockMetrics.Count == response.Count)
			{
				for (int i = 0; i < mockMetrics.Count; i++)
				{
					if ((mockMetrics[i].Value != response[i].Value) ||
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