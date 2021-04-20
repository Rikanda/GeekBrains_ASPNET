using MetricsManagerClient.Models.Metrics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManagerClient.Models
{
	public class AllCpuMetrics : INotifyPropertyChanged
	{
		private readonly int Amount = 10;

		public event PropertyChangedEventHandler PropertyChanged;

		public int AgentId { get; set; }

		public List<CpuMetric> Metrics { get; set; }

		public AllCpuMetrics()
		{
			AgentId = 1;
			Metrics = new List<CpuMetric>();

			var newMetric = new CpuMetric() { Time = DateTimeOffset.UtcNow, Value = 0 };

			for (int i = 0; i < Amount; i++)
			{
				Metrics.Add(newMetric);
				newMetric.Time -= TimeSpan.FromSeconds(5);
			}

			
		}

		public DateTimeOffset LastTime
		{
			get
			{
				return Metrics.Last().Time;
			}
		}

		public void AddMetric(int value, DateTimeOffset time)
		{
			Metrics.Add(new CpuMetric() { Value = value, Time = time });
			Metrics.RemoveAt(0);
		}

		public List<CpuMetric> GetMetrics(int amount, DateTimeOffset time)
		{
			var newMetricsList = new List<CpuMetric>();

			for (int i = 0; i < amount; i++)
			{

			}

			return newMetricsList;
		}

		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}

	}


}
