using AutoMapper;
using MetricsManager.DAL;
using MetricsManager.Responses;

namespace MetricsManager
{
	/// <summary>
	/// Профайлер для маппинга между моделями и DTO объектами метрик
	/// </summary>
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<CpuMetric, CpuMetricDto>();
			//CreateMap<DotNetMetric, DotNetMetricDto>();
			//CreateMap<HddMetric, HddMetricDto>();
			//CreateMap<NetworkMetric, NetworkMetricDto>();
			//CreateMap<RamMetric, RamMetricDto>();
		}
	}
}
