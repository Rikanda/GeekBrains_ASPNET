﻿using AutoMapper;
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
	public class HddControllerUnitTests
	{
		private HddMetricsController controller;
		private Mock<ILogger<HddMetricsController>> mockLogger;
		private Mock<IHddMetricsRepository> mockRepository;

		public HddControllerUnitTests()
		{
			mockLogger = new Mock<ILogger<HddMetricsController>>();
			mockRepository = new Mock<IHddMetricsRepository>();

			var myProfile = new MapperProfile();
			var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
			var mapper = new Mapper(configuration);

			controller = new HddMetricsController(mockLogger.Object, mockRepository.Object, mapper);
		}

		[Fact]
		public void GetMetricsByInterval_ReturnsCorrectMetrics()
		{
			//Arrange
			var request = new HddMetricGetByIntervalRequest()
			{
				FromTime = DateTimeOffset.MinValue,
				ToTime = DateTimeOffset.Now
			};

			//фейковые метрики возвращаемые репозиторием
			var mockMetrics = new List<HddMetric>()
			{
				{ new HddMetric() {Time = DateTimeOffset.MinValue, Value = 100 } },
				{ new HddMetric() {Time = DateTimeOffset.UtcNow, Value = 121 } }
			};

			mockRepository.
				Setup(repository => repository.GetByTimeInterval(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).
				Returns(mockMetrics);

			//Act
			var result = controller.GetMetrics(request);

			var response = ((result as OkObjectResult).Value as AllHddMetricsResponse).Metrics;

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