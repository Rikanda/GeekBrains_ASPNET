using MetricsAgent.DAL;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
	public class CpuMetricJob : IJob
	{
		private readonly IServiceProvider _provider;
		private ICpuMetricsRepository repository;

		public CpuMetricJob(IServiceProvider provider)
		{
			_provider = provider;
			repository = _provider.GetService<ICpuMetricsRepository>();
		}

		public Task Execute(IJobExecutionContext context)
		{
			return Task.CompletedTask;
		}
	}
}
