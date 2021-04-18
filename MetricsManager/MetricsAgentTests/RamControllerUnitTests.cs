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
	public class RamControllerUnitTests
	{
		private RamMetricsController controller;
		private Mock<ILogger<RamMetricsController>> mockLogger;
		private Mock<IRamMetricsRepository> mockRepository;

		public RamControllerUnitTests()
		{
			mockLogger = new Mock<ILogger<RamMetricsController>>();
			mockRepository = new Mock<IRamMetricsRepository>();

			var myProfile = new MapperProfile();
			var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
			var mapper = new Mapper(configuration);

			controller = new RamMetricsController(mockLogger.Object, mockRepository.Object, mapper);
		}

		[Fact]
		public void GetMetricsByInterval_ReturnsCorrectMetrics()
		{
			//Arrange
			var request = new RamMetricGetByIntervalRequest()
			{
				FromTime = DateTimeOffset.MinValue,
				ToTime = DateTimeOffset.Now
			};

			//фейковые метрики возвращаемые репозиторием
			var mockMetrics = new List<RamMetric>()
			{
				{ new RamMetric() {Time = DateTimeOffset.MinValue, Value = 100 } },
				{ new RamMetric() {Time = DateTimeOffset.UtcNow, Value = 121 } }
			};

			mockRepository.
				Setup(repository => repository.GetByTimeInterval(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).
				Returns(mockMetrics);

			//Act
			var result = controller.GetMetrics(request);

			var response = ((result as OkObjectResult).Value as AllRamMetricsResponse).Metrics;

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


		[Fact]
		public void GetMetricsLast_ReturnsCorrectMetrics()
		{
			//Arrange
			//фейковая метрика возвращаемая репозиторием
			var mockMetric = new RamMetric() { Time = DateTimeOffset.MinValue, Value = 100 };
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
				if ((mockMetric.Value != response.Value) ||
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