using AutoMapper;
using Metrics.Tools;
using MetricsManager;
using MetricsManager.Controllers;
using MetricsManager.DAL;
using MetricsManager.Requests;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MetricsManagerTests
{
	public class RamControllerUnitTests
	{
		private RamMetricsController controller;
		private Mock<ILogger<RamMetricsController>> mockLogger;
		private Mock<IRamMetricsRepository> mockRepository;
		private Mock<IAgentsRepository> mockAgentsRepository;

		public RamControllerUnitTests()
		{
			mockLogger = new Mock<ILogger<RamMetricsController>>();
			mockRepository = new Mock<IRamMetricsRepository>();
			mockAgentsRepository = new Mock<IAgentsRepository>();

			var myProfile = new MapperProfile();
			var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
			var mapper = new Mapper(configuration);

			controller = new RamMetricsController(
				mockLogger.Object,
				mockRepository.Object,
				mockAgentsRepository.Object,
				mapper);
		}

		[Fact]
		public void GetMetricsFromAgent_ReturnsCorrectMetrics()
		{
			//Arrange
			var request = new RamMetricGetByIntervalForAgentRequest()
			{
				agentId = 1,
				fromTime = DateTimeOffset.MinValue,
				toTime = DateTimeOffset.Now
			};

			//фейковые метрики возвращаемые репозиторием
			var mockMetrics = new AllMetrics<RamMetric>();
			mockMetrics.Metrics.Add(new RamMetric() { AgentId = 1, Time = DateTimeOffset.MinValue, Value = 100 });
			mockMetrics.Metrics.Add(new RamMetric() { AgentId = 1, Time = DateTimeOffset.UtcNow, Value = 121 });

			mockRepository.
				Setup(repository => repository.GetByTimeInterval(It.IsAny<int>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).
				Returns(mockMetrics);

			//Act
			var result = controller.GetMetricsFromAgent(request);

			// Assert
			Assert.True(CompareMetricsContainers(result, mockMetrics));

		}

		[Fact]
		public void GetMetricsByPercentileFromAgent_ReturnsCorrectMetric()
		{
			//Arrange
			var request = new RamMetricGetByIntervalForAgentRequest()
			{
				agentId = 1,
				fromTime = DateTimeOffset.MinValue,
				toTime = DateTimeOffset.Now
			};
			var percentile = Percentile.P90;

			//фейковые метрики возвращаемые репозиторием
			var mockMetrics = new AllMetrics<RamMetric>();
			mockMetrics.Metrics.Add(new RamMetric() { AgentId = 1, Time = DateTimeOffset.MinValue, Value = 100 });

			mockRepository.
				Setup(repository => repository.GetByTimeIntervalPercentile(
					It.IsAny<int>(),
					It.IsAny<DateTimeOffset>(),
					It.IsAny<DateTimeOffset>(),
					It.IsAny<Percentile>())).
				Returns(mockMetrics);

			//Act
			var result = controller.GetMetricsByPercentileFromAgent(request, percentile);

			// Assert
			Assert.True(CompareMetricsContainers(result, mockMetrics));

		}

		[Fact]
		public void GetMetricsFromAllCluster_ReturnsCorrectMetrics()
		{
			//Arrange
			var request = new RamMetricGetByIntervalForClusterRequest()
			{
				fromTime = DateTimeOffset.MinValue,
				toTime = DateTimeOffset.Now
			};

			//фейковые данные об агентах
			var mockAgentsInfo = new AllAgentsInfo();
			mockAgentsInfo.Agents.Add(new AgentInfo() { AgentId = 1, AgentUri = "url1" });
			mockAgentsInfo.Agents.Add(new AgentInfo() { AgentId = 2, AgentUri = "url2" });

			mockAgentsRepository.
				Setup(repository => repository.GetAllAgentsInfo()).
				Returns(mockAgentsInfo);

			//фейковые метрики возвращаемые репозиторием для всех агентов
			var mockMetrics = new AllMetrics<RamMetric>();
			mockMetrics.Metrics.Add(new RamMetric() { AgentId = 1, Time = DateTimeOffset.MinValue, Value = 100 });
			mockMetrics.Metrics.Add(new RamMetric() { AgentId = 1, Time = DateTimeOffset.UtcNow, Value = 121 });
			mockMetrics.Metrics.Add(new RamMetric() { AgentId = 2, Time = DateTimeOffset.MinValue, Value = 101 });
			mockMetrics.Metrics.Add(new RamMetric() { AgentId = 2, Time = DateTimeOffset.UtcNow, Value = 122 });

			//фейковые метрики возвращаемые репозиторием для первого агента
			var mockMetricsForAgent_1 = new AllMetrics<RamMetric>();
			mockMetricsForAgent_1.Metrics.Add(mockMetrics.Metrics[0]);
			mockMetricsForAgent_1.Metrics.Add(mockMetrics.Metrics[1]);
			//фейковые метрики возвращаемые репозиторием для второго агента
			var mockMetricsForAgent_2 = new AllMetrics<RamMetric>();
			mockMetricsForAgent_2.Metrics.Add(mockMetrics.Metrics[2]);
			mockMetricsForAgent_2.Metrics.Add(mockMetrics.Metrics[3]);

			mockRepository.
				Setup(repository => repository.GetByTimeInterval(
					It.Is<int>(agentId => agentId == 1),
					It.IsAny<DateTimeOffset>(),
					It.IsAny<DateTimeOffset>())).
				Returns(mockMetricsForAgent_1);
			mockRepository.
				Setup(repository => repository.GetByTimeInterval(
					It.Is<int>(agentId => agentId == 2),
					It.IsAny<DateTimeOffset>(),
					It.IsAny<DateTimeOffset>())).
				Returns(mockMetricsForAgent_2);

			//Act
			var result = controller.GetMetricsFromAllCluster(request);

			// Assert
			Assert.True(CompareMetricsContainers(result, mockMetrics));

		}

		[Fact]
		public void GetMetricsByPercentileFromAllCluster_ReturnsOk()
		{
			//Arrange
			var request = new RamMetricGetByIntervalForClusterRequest()
			{
				fromTime = DateTimeOffset.MinValue,
				toTime = DateTimeOffset.Now
			};
			var percentile = Percentile.P95;

			//фейковые данные об агентах
			var mockAgentsInfo = new AllAgentsInfo();
			mockAgentsInfo.Agents.Add(new AgentInfo() { AgentId = 1, AgentUri = "url1" });
			mockAgentsInfo.Agents.Add(new AgentInfo() { AgentId = 2, AgentUri = "url2" });

			mockAgentsRepository.
				Setup(repository => repository.GetAllAgentsInfo()).
				Returns(mockAgentsInfo);

			//фейковые метрики возвращаемые репозиторием для всех агентов
			var mockMetrics = new AllMetrics<RamMetric>();
			mockMetrics.Metrics.Add(new RamMetric() { AgentId = 1, Time = DateTimeOffset.MinValue, Value = 100 });
			mockMetrics.Metrics.Add(new RamMetric() { AgentId = 2, Time = DateTimeOffset.UtcNow, Value = 122 });

			//фейковые метрики возвращаемые репозиторием для первого агента
			var mockMetricsForAgent_1 = new AllMetrics<RamMetric>();
			mockMetricsForAgent_1.Metrics.Add(mockMetrics.Metrics[0]);
			//фейковые метрики возвращаемые репозиторием для второго агента
			var mockMetricsForAgent_2 = new AllMetrics<RamMetric>();
			mockMetricsForAgent_2.Metrics.Add(mockMetrics.Metrics[1]);

			mockRepository.
				Setup(repository => repository.GetByTimeIntervalPercentile(
					It.Is<int>(agentId => agentId == 1),
					It.IsAny<DateTimeOffset>(),
					It.IsAny<DateTimeOffset>(),
					It.IsAny<Percentile>())).
				Returns(mockMetricsForAgent_1);
			mockRepository.
				Setup(repository => repository.GetByTimeIntervalPercentile(
					It.Is<int>(agentId => agentId == 2),
					It.IsAny<DateTimeOffset>(),
					It.IsAny<DateTimeOffset>(),
					It.IsAny<Percentile>())).
				Returns(mockMetricsForAgent_2);

			//Act
			var result = controller.GetMetricsByPercentileFromAllCluster(request, percentile);

			// Assert
			Assert.True(CompareMetricsContainers(result, mockMetrics));
		}

		/// <summary>
		/// Сравнивает содержимое мок контейнера с метриками и контейнера с 
		/// метриками которые содержатся в результате возвращаемом из контроллера
		/// </summary>
		/// <param name="result">Ответ возвращаемый контроллером</param>
		/// <param name="mockMetrics">Мок контейнер с метриками</param>
		/// <returns>true если содержимое контейнеров полностью совпадает</returns>
		private bool CompareMetricsContainers(IActionResult result, AllMetrics<RamMetric> mockMetrics)
		{
			var responseMetrics = ((result as OkObjectResult).Value as AllMetricsResponse<RamMetricDto>);

			bool check = true;
			if (mockMetrics.Metrics.Count == responseMetrics.Metrics.Count)
			{
				for (int i = 0; i < mockMetrics.Metrics.Count; i++)
				{
					if ((mockMetrics.Metrics[i].Value != responseMetrics.Metrics[i].Value) ||
						(mockMetrics.Metrics[i].Time != responseMetrics.Metrics[i].Time) ||
						(mockMetrics.Metrics[i].AgentId != responseMetrics.Metrics[i].AgentId))
					{
						check = false;//Если хоть одоин элемент в любой паре метрик не совпадает - проверка провалена
					}
				}
			}
			else//Если длина контейнеров не совпадает - проверка провалена
			{
				check = false;
			}

			return check;
		}


	}
}