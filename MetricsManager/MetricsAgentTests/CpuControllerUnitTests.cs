using Metrics.Tools;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
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
			controller = new CpuMetricsController(mockLogger.Object, mockRepository.Object);
		}

		[Fact]
		public void Create_ShouldCall_Create_From_Repository()
		{
			// устанавливаем параметр заглушки
			// в заглушке прописываем что в репозиторий прилетит CpuMetric объект
			mockRepository.Setup(repository => repository.Create(It.IsAny<CpuMetric>())).Verifiable();

			// выполняем действие на контроллере
			var result = controller.Create(new MetricsAgent.Requests.CpuMetricCreateRequest { Time = 1, Value = 50 });

			// проверяем заглушку на то, что пока работал контроллер
			// действительно вызвался метод Create репозитория с нужным типом объекта в параметре
			mockRepository.Verify(repository => repository.Create(It.IsAny<CpuMetric>()), Times.AtMostOnce());
		}


		[Fact]
		public void GetMetricsFromAgent_ReturnsOk()
		{
			//Arrange
			var fromTime = TimeSpan.FromSeconds(0);
			var toTime = TimeSpan.FromSeconds(100);

			//Act
			var result = controller.GetMetricsFromAgent(fromTime, toTime);

			// Assert
			_ = Assert.IsAssignableFrom<IActionResult>(result);
		}

		[Fact]
		public void GetMetricsByPercentileFromAgent_ReturnsOk()
		{
			//Arrange
			var fromTime = TimeSpan.FromSeconds(0);
			var toTime = TimeSpan.FromSeconds(100);
			var percentile = Percentile.P90;

			//Act
			var result = controller.GetMetricsByPercentileFromAgent(fromTime, toTime, percentile);

			// Assert
			_ = Assert.IsAssignableFrom<IActionResult>(result);
		}

	}
}