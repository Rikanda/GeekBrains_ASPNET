using AutoMapper;
using MetricsAgent;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.DAL.Models;
using MetricsAgent.DAL.Repositories;
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
	public class DotNetControllerUnitTests
	{
		private DotNetMetricsController controller;
		private Mock<ILogger<DotNetMetricsController>> mockLogger;
		private Mock<IDotNetMetricsRepository> mockRepository;

		public DotNetControllerUnitTests()
		{
			mockLogger = new Mock<ILogger<DotNetMetricsController>>();
			mockRepository = new Mock<IDotNetMetricsRepository>();

			var myProfile = new MapperProfile();
			var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
			var mapper = new Mapper(configuration);

			controller = new DotNetMetricsController(mockLogger.Object, mockRepository.Object, mapper);
		}

		[Fact]
		public void GetMetricsByInterval_ReturnsCorrectMetrics()
		{
			//Arrange
			//Arrange
			var request = new DotNetMetricGetByIntervalRequest()
			{
				FromTime = DateTimeOffset.MinValue,
				ToTime = DateTimeOffset.Now
			};

			//фейковые метрики возвращаемые репозиторием
			var mockMetrics = new List<DotNetMetric>()
			{
				{ new DotNetMetric() {Time = DateTimeOffset.MinValue, Value = 100 } },
				{ new DotNetMetric() {Time = DateTimeOffset.UtcNow, Value = 121 } }
			};
			mockRepository.
				Setup(repository => repository.GetByTimeInterval(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).
				Returns(mockMetrics);

			//Act
			var result = controller.GetMetrics(request);

			var response = ((result as OkObjectResult).Value as AllDotNetMetricsResponse).Metrics;

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