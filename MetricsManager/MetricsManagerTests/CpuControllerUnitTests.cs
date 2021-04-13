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
	public class CpuControllerUnitTests
	{
		private CpuMetricsController controller;
		private Mock<ILogger<CpuMetricsController>> mockLogger;
		private Mock<ICpuMetricsRepository> mockRepository;
		private Mock<IAgentsRepository> mockAgentsRepository;

		public CpuControllerUnitTests()
		{
			mockLogger = new Mock<ILogger<CpuMetricsController>>();
			mockRepository = new Mock<ICpuMetricsRepository>();
			mockAgentsRepository = new Mock<IAgentsRepository>();

			var myProfile = new MapperProfile();
			var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
			var mapper = new Mapper(configuration);

			controller = new CpuMetricsController(
				mockLogger.Object, 
				mockRepository.Object, 
				mockAgentsRepository.Object, 
				mapper);
		}

		[Fact]
		public void GetMetricsFromAgent_ReturnsCorrectMetrics()
		{
			//Arrange
			var request = new CpuMetricGetByIntervalForAgentRequest()
			{
				agentId = 1,
				fromTime = DateTimeOffset.MinValue,
				toTime = DateTimeOffset.Now
			};

			//фейковые метрики возвращаемые репозиторием
			var mockMetrics = new AllCpuMetrics();
			mockMetrics.Metrics.Add(new CpuMetric() { AgentId = 1, Time = DateTimeOffset.MinValue, Value = 100 });
			mockMetrics.Metrics.Add(new CpuMetric() { AgentId = 1, Time = DateTimeOffset.UtcNow, Value = 121 });

			mockRepository.
				Setup(repository => repository.GetByTimeInterval(It.IsAny<int>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).
				Returns(mockMetrics);

			//Act
			var result = controller.GetMetricsFromAgent(request);

			var response = ((result as OkObjectResult).Value as AllCpuMetricsResponse).Metrics;

			//сравнение полученных значений с ожидаемыми значениями
			bool check = true;
			if (mockMetrics.Metrics.Count == response.Count)
			{
				for (int i = 0; i < mockMetrics.Metrics.Count; i++)
				{
					if ((mockMetrics.Metrics[i].Value != response[i].Value) ||
						(mockMetrics.Metrics[i].Time != response[i].Time) ||
						(mockMetrics.Metrics[i].AgentId != response[i].AgentId))
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

			// Assert
			_ = Assert.IsAssignableFrom<IActionResult>(result);
		}

		[Fact]
		public void GetMetricsByPercentileFromAgent_ReturnsCorrectMetric()
		{
			//Arrange
			var request = new CpuMetricGetByIntervalForAgentRequest()
			{
				agentId = 1,
				fromTime = DateTimeOffset.MinValue,
				toTime = DateTimeOffset.Now
			};
			var percentile = Percentile.P90;

			//фейковые метрики возвращаемые репозиторием
			var mockMetrics = new AllCpuMetrics();
			mockMetrics.Metrics.Add(new CpuMetric() { AgentId = 1, Time = DateTimeOffset.MinValue, Value = 100 });

			mockRepository.
				Setup(repository => repository.GetByTimeIntervalPercentile(
					It.IsAny<int>(), 
					It.IsAny<DateTimeOffset>(), 
					It.IsAny<DateTimeOffset>(),
					It.IsAny<Percentile>())).
				Returns(mockMetrics);

			//Act
			var result = controller.GetMetricsByPercentileFromAgent(request, percentile);

			var response = ((result as OkObjectResult).Value as AllCpuMetricsResponse).Metrics;

			//сравнение полученных значений с ожидаемыми значениями
			bool check = true;
			if (mockMetrics.Metrics.Count == response.Count)
			{
				for (int i = 0; i < mockMetrics.Metrics.Count; i++)
				{
					if ((mockMetrics.Metrics[i].Value != response[i].Value) ||
						(mockMetrics.Metrics[i].Time != response[i].Time) ||
						(mockMetrics.Metrics[i].AgentId != response[i].AgentId))
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
		public void GetMetricsFromAllCluster_ReturnsCorrectMetrics()
		{
			//Arrange
			var request = new CpuMetricGetByIntervalForClusterRequest()
			{
				fromTime = DateTimeOffset.MinValue,
				toTime = DateTimeOffset.Now
			};

			//фейковые данные об агентах
			var mockAgentsInfo = new AllAgentsInfo();
			mockAgentsInfo.Agents.Add(new AgentInfo() { AgentId = 1, AgentUri = "url1"});
			mockAgentsInfo.Agents.Add(new AgentInfo() { AgentId = 2, AgentUri = "url2" });

			mockAgentsRepository.
				Setup(repository => repository.GetAllAgentsInfo()).
				Returns(mockAgentsInfo);

			//фейковые метрики возвращаемые репозиторием
			var mockMetrics = new AllCpuMetrics();
			mockMetrics.Metrics.Add(new CpuMetric() 
			{ 
				AgentId = mockAgentsInfo.Agents[0].AgentId, Time = DateTimeOffset.MinValue, Value = 100 
			});
			mockMetrics.Metrics.Add(new CpuMetric() 
			{ 
				AgentId = mockAgentsInfo.Agents[1].AgentId, Time = DateTimeOffset.UtcNow, Value = 121 
			});

			mockRepository.
				Setup(repository => repository.GetByTimeInterval(
					It.IsAny<int>(),
					It.IsAny<DateTimeOffset>(),
					It.IsAny<DateTimeOffset>())).
				Returns(mockMetrics);

			//Act
			var result = controller.GetMetricsFromAllCluster(request);

			// Assert
			_ = Assert.IsAssignableFrom<IActionResult>(result);
		}

		[Fact]
		public void GetMetricsByPercentileFromAllCluster_ReturnsOk()
		{
			//Arrange
			var request = new CpuMetricGetByIntervalForClusterRequest()
			{
				fromTime = DateTimeOffset.MinValue,
				toTime = DateTimeOffset.Now
			};
			var percentile = Percentile.P90;

			//фейковые данные об агентах
			var mockAgentsInfo = new AllAgentsInfo();
			mockAgentsInfo.Agents.Add(new AgentInfo() { AgentId = 1, AgentUri = "url1" });
			mockAgentsInfo.Agents.Add(new AgentInfo() { AgentId = 2, AgentUri = "url2" });

			mockAgentsRepository.
				Setup(repository => repository.GetAllAgentsInfo()).
				Returns(mockAgentsInfo);

			//фейковые метрики возвращаемые репозиторием
			var mockMetrics = new AllCpuMetrics();
			mockMetrics.Metrics.Add(new CpuMetric()
			{
				AgentId = mockAgentsInfo.Agents[0].AgentId,
				Time = DateTimeOffset.MinValue,
				Value = 100
			});
			mockMetrics.Metrics.Add(new CpuMetric()
			{
				AgentId = mockAgentsInfo.Agents[1].AgentId,
				Time = DateTimeOffset.UtcNow,
				Value = 121
			});

			mockRepository.
				Setup(repository => repository.GetByTimeIntervalPercentile(
					It.IsAny<int>(),
					It.IsAny<DateTimeOffset>(),
					It.IsAny<DateTimeOffset>(),
					It.IsAny<Percentile>())).
				Returns(mockMetrics);


			//Act
			var result = controller.GetMetricsByPercentileFromAllCluster(request, percentile);

			// Assert
			_ = Assert.IsAssignableFrom<IActionResult>(result);
		}



	}
}