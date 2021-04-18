using AutoMapper;
using MetricsAgent.DAL;
using MetricsAgent.DAL.Models;
using MetricsAgent.Responses;

namespace MetricsAgent
{
	/// <summary>
	/// Профайлер для маппинга между моделями и DTO объектами метрик
	/// </summary>
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<CpuMetric, CpuMetricDto>();
			CreateMap<DotNetMetric, DotNetMetricDto>();
			CreateMap<HddMetric, HddMetricDto>();
			CreateMap<NetworkMetric, NetworkMetricDto>();
			CreateMap<RamMetric, RamMetricDto>();
		}
	}
}
