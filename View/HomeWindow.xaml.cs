using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows;
using LiveCharts;
using Netizen.Controller;

namespace Netizen.View
{
  [SuppressMessage("ReSharper", "FunctionNeverReturns")]
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

      try
      {
        WalletTextBot.Text = Application.Current.Properties["usernameWeb"].ToString();
      }
      catch (Exception e)
      {
        WalletTextBot.Text = e.Message;
        Console.WriteLine(e);
        throw;
      }
    }

    private void StartBotA(Object sender, RoutedEventArgs routedEventArgs)
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
            var body = new Dictionary<String, String>
            {
              {"a", "PlaceBet"},
              {"s", "8ed4a55004bf46139589a9ea3656f4f9"},
              {"Low", "0"},
              {"High", "899999"},
              {"PayIn", "0"},
              {"ProtocolVersion", "2"},
              {"ClientSeed", seed},
              {"Currency", "doge"}
            };
            var response = await new DogeController().Post(body);
            if ((int) response["code"] == 200)
            {
              seed = (String) response.GetValue("data")?["Next"];
              if (_iChartValue.Count >= 40)
              {
                _iChartValue.RemoveAt(0);
              }

              _iChartValue.Add((long) response["data"]?["BetId"] * 0.00000001);
              Dispatcher.Invoke(() =>
              {
                Chart.Series[0].Values = _iChartValue;
                WalletTextBot.Text     = (String) response.GetValue("data")?["Next"] ?? string.Empty;
              });
            }
            else
            {
              Dispatcher.Invoke(() => { WalletTextBot.Text = (String) response.GetValue("data") ?? string.Empty; });
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