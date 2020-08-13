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
      UsernameBox.Text = (string) Application.Current.Properties["usernameWeb"];
    }

    private void LoginClick(object sender, RoutedEventArgs routedEventArgs)
    {
      Application.Current.Properties["usernameWeb"] = UsernameBox.Text;
      var home = new HomeWindow();
      home.Show();
      Close();
    }
  }
}