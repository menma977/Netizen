using System.Windows;

namespace Netizen.View
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void LoginClick(object sender, RoutedEventArgs routedEventArgs)
    {
      var home = new HomeWindow();
      home.Show();
      Close();
    }
  }
}