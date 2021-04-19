using Microsoft.Extensions.Logging;
using System;
using System.Windows;

namespace MetricsManagerClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly ILogger _logger;

		public MainWindow(ILogger<MainWindow> logger)
		{
			InitializeComponent();
			_logger = logger;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			_logger.LogDebug("Button clicked by " +
				$"{sender.ToString()}");

			CpuChart.ColumnServiesValues[0].Values.Add(new Random().NextDouble()*100);
			CpuChart.ColumnServiesValues[0].Values.RemoveAt(0);
		}

		private void CpuChart_Loaded(object sender, RoutedEventArgs e)
		{
			_logger.LogDebug("CpuChart loaded by " +
				$"{sender.ToString()}");
		}
	}
}