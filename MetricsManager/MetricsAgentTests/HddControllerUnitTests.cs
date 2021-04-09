using AutoMapper;
using MetricsAgent;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
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
		public void GetMetrics_ReturnsOk()
		{
			//Arrange
			//фейковая метрика возвращаемая репозиторием
			var mockMetric = new HddMetric() { Id = 1, Time = TimeSpan.Zero, Value = 100 };
			mockRepository.
				Setup(repository => repository.GetLast()).
				Returns(mockMetric);

			//Act
			var result = controller.GetMetrics();

			var response = (result as OkObjectResult).Value as HddMetricDto;

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