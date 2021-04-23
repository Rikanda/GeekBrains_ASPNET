using LiveCharts;
using LiveCharts.Wpf;
using MetricsManagerClient.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace MetricsManagerClient.Charts
{
	/// <summary>
	/// Interaction logic for MaterialCards.xaml
	/// </summary>
	public partial class MaterialCardsRam : UserControl, INotifyPropertyChanged
	{
		private  readonly string maxValue;
		private string currentMetric;
		private string currentTime;

		public string MaxValue
		{
			get { return maxValue; }
		}

		public string CurrentMetric
		{
			get { return currentMetric; }
			set
			{
				currentMetric = value;
				OnPropertyChanged("CurrentMetric");
			}
		}

		public string CurrentTime
		{
			get { return currentTime; }
			set
			{
				currentTime = value;
				OnPropertyChanged("CurrentTime");
			}
		}

		public MaterialCardsRam()
		{
			maxValue = (new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory >> 20).ToString();
			InitializeComponent();

			ColumnServiesValues = new SeriesCollection
			{
				new ColumnSeries
				{
					Values = new ChartValues<int> { }
				}
			};

			CurrentMetric = "0";

			DataContext = this;

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