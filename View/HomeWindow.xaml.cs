using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using LiveCharts;
using Netizen.Collection;
using Netizen.Config;
using Netizen.Model;
using RestSharp;
using RestSharp.Serialization.Json;

namespace Netizen.View
{
  public partial class HomeWindow
  {
    readonly ChartValues<double> _iChartValue = new ChartValues<Double>();
    private  double              Balance      = 1000.000000000;


    public HomeWindow()
    {
      InitializeComponent();
      _iChartValue.Add(Balance);
      Chart.Series[0].Values = _iChartValue;
      Chart.Update(true);
    }

    private void UpdateOnclick(object sender, RoutedEventArgs routedEventArgs)
    {
      new Thread(_thread).Start();
    }

    private void GoToPage(object sender, RoutedEventArgs routedEventArgs)
    {
      _iChartValue.RemoveAt(0);
      _iChartValue.Add(Balance * 0.00000001);
      Chart.Series[0].Values = _iChartValue;
    }

    private async void _thread()
    {
      var seed = "0";
      var time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
      while (true)
      {
        var delta = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - time;
        if (delta > 1000)
        {
          time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
          try
          {
            var body = new Dictionary<String, String>();
            body.Add("a",               "PlaceBet");
            body.Add("s",               "8ed4a55004bf46139589a9ea3656f4f9");
            body.Add("Low",             "0");
            body.Add("High",            "899999");
            body.Add("PayIn",           "0");
            body.Add("ProtocolVersion", "2");
            body.Add("ClientSeed",      seed);
            body.Add("Currency",        "doge");
            var dogeController = await new DogeController().Post(body);
            if ((int) dogeController["code"] == 200)
            {
              seed = (String) dogeController.GetValue("data")["Next"];


              if (_iChartValue.Count >= 40)
              {
                _iChartValue.RemoveAt(0);
              }

              _iChartValue.Add((long) dogeController["data"]["BetId"] * 0.00000001);
              Dispatcher.Invoke(() => { Chart.Series[0].Values = _iChartValue; });

              Dispatcher.Invoke(() => { WalletTextBot.Text = (String) dogeController.GetValue("data")["Next"]; });
            }
            else
            {
              Dispatcher.Invoke(() => { WalletTextBot.Text = "Sleep" + dogeController.First; });
              Thread.Sleep(60000);
            }
          }
          catch (Exception e)
          {
            Dispatcher.Invoke(() => { WalletTextBot.Text = e.Message; });
            Thread.Sleep(60000);
          }
        }
      }
    }
  }
}