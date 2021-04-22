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
	public partial class MaterialCardsCpu : UserControl, INotifyPropertyChanged
	{
		private string currentLoad;
		private string currentTime;
		public string CurrentLoad 
		{
			get { return currentLoad; }
			set 
			{
				currentLoad = value;
				OnPropertyChanged("CurrentLoad");
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

		public MaterialCardsCpu()
		{
			InitializeComponent();

			ColumnServiesValues = new SeriesCollection
			{
				new ColumnSeries
				{
					Values = new ChartValues<int> { }
				}
			};

			CurrentLoad = "0";

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