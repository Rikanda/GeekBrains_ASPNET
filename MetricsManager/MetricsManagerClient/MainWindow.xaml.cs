using MetricsManagerClient.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

		private void CpuChart_Loaded(object sender, RoutedEventArgs e)
		{
			_logger.LogDebug("CpuChart loaded by " +
				$"{sender.ToString()}");
		}
	}
}