using MetricsManagerClient.Models;
using MetricsManagerClient.Charts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace MetricsManagerClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>Количество метрик отображающихся на графике</summary>
		private readonly int Amount = 12;

		private readonly IServiceProvider _provider;
		private readonly ILogger _logger;
		private IAllCpuMetrics _allCpuMetrics;
		private IAllRamMetrics _allRamMetrics;

		public MainWindow(ILogger<MainWindow> logger, IServiceProvider provider)
		{
			InitializeComponent();
			_logger = logger;

			_provider = provider;
			_allCpuMetrics = _provider.GetService<IAllCpuMetrics>();
			_allRamMetrics = _provider.GetService<IAllRamMetrics>();

			//Подписка на событие изменения значений метрик в модели
			_allCpuMetrics.OnMetricsChange += OnCpuDataChange;
			_allRamMetrics.OnMetricsChange += OnRamDataChange;
		}

		public void OnCpuDataChange(object sender, EventArgs e)
		{
			_logger.LogDebug("Cpu data changed ");

			List<int> values = _allCpuMetrics.GetMetricsValues(Amount);

			CpuChart.ColumnServiesValues[0].Values.Clear();

			foreach (var value in values)
			{
				CpuChart.ColumnServiesValues[0].Values.Add((int)value);
			}

			CpuChart.CurrentMetric = values[values.Count - 1].ToString();
			CpuChart.CurrentTime = DateTimeOffset.UtcNow.ToString("G");

		}

		public void OnRamDataChange(object sender, EventArgs e)
		{
			_logger.LogDebug("Ram data changed ");

			List<int> values = _allRamMetrics.GetMetricsValues(Amount);

			RamChart.ColumnServiesValues[0].Values.Clear();

			foreach (var value in values)
			{
				RamChart.ColumnServiesValues[0].Values.Add((int)value);
			}

			RamChart.CurrentMetric = values[values.Count - 1].ToString();
			RamChart.CurrentTime = DateTimeOffset.UtcNow.ToString("G");

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