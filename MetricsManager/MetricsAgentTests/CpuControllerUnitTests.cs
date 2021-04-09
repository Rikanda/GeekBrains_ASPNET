using AutoMapper;
using Metrics.Tools;
using MetricsAgent;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

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

			var myProfile = new MapperProfile();
			var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
			var mapper = new Mapper(configuration);

			controller = new CpuMetricsController(mockLogger.Object, mockRepository.Object, mapper);
		}

		[Fact]
		public void GetMetrics_ReturnsOk()
		{
			//Arrange
			var fromTime = DateTimeOffset.MinValue;
			var toTime = DateTimeOffset.Now;

			//�������� ������� ������������ ������������
			var mockMetrics = new List<CpuMetric>()
			{
				{ new CpuMetric() {Time = TimeSpan.FromDays(5), Value = 100 } },
				{ new CpuMetric() {Time = TimeSpan.FromDays(10), Value = 100 } }
			};

			mockRepository.
				Setup(repository => repository.GetByTimeInterval(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).
				Returns(mockMetrics);

			//Act
			var result = controller.GetMetrics(fromTime, toTime);

			var response = ((result as OkObjectResult).Value as AllCpuMetricsResponse).Metrics;

			//��������� ���������� �������� � ���������� ����������
			bool check = true;
			if(mockMetrics.Count == response.Count)
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