using LiveCharts;
using LiveCharts.Wpf;
using MetricsManagerClient.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MetricsManagerClient
{
	/// <summary>
	/// Interaction logic for MaterialCards.xaml
	/// </summary>
	public partial class RamMaterialCards : UserControl, INotifyPropertyChanged
	{
		/// <summary>Количество метрик отображающихся на графике</summary>
		private readonly int Amount = 12;

		private readonly ILogger _logger;
		private AllCpuMetrics _allCpuMetrics;

		public RamMaterialCards(ILogger<MainWindow> logger, AllCpuMetrics allCpuMetrics)
		{
			InitializeComponent();

			ColumnServiesValues = new SeriesCollection
			{
				new ColumnSeries
				{
					Values = new ChartValues<double> { }
				}
			};

			DataContext = this;

			_logger = logger;
			_allCpuMetrics = allCpuMetrics;
			//Подписка на событие изменения значений метрик в модели
			allCpuMetrics.onMetricsChange += onDataChange;
		}

		public void onDataChange()
		{
			_logger.LogDebug("Data changed ");

			List<int> values = _allCpuMetrics.GetMetricsValues(Amount);

			ColumnServiesValues[0].Values.Clear();
			foreach (var value in values)
			{
				ColumnServiesValues[0].Values.Add((double)value);
			}

		}

		public SeriesCollection ColumnServiesValues { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}