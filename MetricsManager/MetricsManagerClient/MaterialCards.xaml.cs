using LiveCharts;
using LiveCharts.Wpf;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MetricsManagerClient
{
	/// <summary>
	/// Interaction logic for MaterialCards.xaml
	/// </summary>
	public partial class MaterialCards : UserControl, INotifyPropertyChanged
	{

		public MaterialCards()
		{
			InitializeComponent();

			ColumnServiesValues = new SeriesCollection
			{
				new ColumnSeries
				{
					Values = new ChartValues<double> { 10,20,30,40,50,60,70,80,90,100 }
				}
			};

			DataContext = this;
		}

		public SeriesCollection ColumnServiesValues { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		private void UpdateOnСlick(object sender, RoutedEventArgs e)
		{
			TimePowerChart.Update(true);
		}
	}
}