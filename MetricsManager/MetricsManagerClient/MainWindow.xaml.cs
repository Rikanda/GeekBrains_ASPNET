using System;
using System.Windows;

namespace MetricsManagerClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			//CpuChart.ColumnServiesValues[0].Values.Add(48d);
			CpuChart.ColumnServiesValues[0].Values.Add(new Random().NextDouble()*100);
			CpuChart.ColumnServiesValues[0].Values.RemoveAt(0);
		}

		private void CpuChart_Loaded(object sender, RoutedEventArgs e)
		{

		}
	}
}