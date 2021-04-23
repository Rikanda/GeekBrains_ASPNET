using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManagerClient.Models.Metrics
{
	public class RamMetric
	{
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}
}
