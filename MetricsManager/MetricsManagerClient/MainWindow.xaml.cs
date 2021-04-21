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
		/// <summary>Количество метрик отображающихся на графике</summary>
		private readonly int Amount = 12;

		private readonly ILogger _logger;
		private AllCpuMetrics _allCpuMetrics;

		public MainWindow(ILogger<MainWindow> logger, AllCpuMetrics allCpuMetrics)
		{
			InitializeComponent();
			_logger = logger;

			_allCpuMetrics = allCpuMetrics;
			//Подписка на событие изменения значений метрик в модели
			allCpuMetrics.onMetricsChange += onCpuDataChange;
		}

		public void onCpuDataChange()
		{
			_logger.LogDebug("Data changed ");

			List<int> values = _allCpuMetrics.GetMetricsValues(Amount);

			CpuChart.ColumnServiesValues[0].Values.Clear();

			foreach (var value in values)
			{
				CpuChart.ColumnServiesValues[0].Values.Add((int)value);
			}

			CpuChart.CurrentLoad = values[values.Count - 1].ToString();
			CpuChart.CurrentTime = DateTimeOffset.UtcNow.ToString("G");

		}

		private void CpuChart_Loaded(object sender, RoutedEventArgs e)
		{
			_logger.LogDebug("CpuChart loaded by " +
				$"{sender.ToString()}");
		}

		private void RamChart_Loaded(object sender, RoutedEventArgs e)
		{
			_logger.LogDebug("CpuChart loaded by " +
				$"{sender.ToString()}");
		}

	}
}