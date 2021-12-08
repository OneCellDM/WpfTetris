using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WpfTetris.Interfaces;

namespace WpfTetris.Pages
{
    public partial class Info : UserControl, ICloseControl
    {
        public Info()
        {
            InitializeComponent();
        }

        public event CloseControl CloseEvent;

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
        }

        private void WpfLink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(
                @"https://docs.microsoft.com/ru-ru/dotnet/desktop/wpf/introduction-to-wpf?view=netframeworkdesktop-4.8");
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseEvent?.Invoke();
        }

		private void webSite_Click(object sender, RoutedEventArgs e)
		{
            Process.Start(
                @"http://spedwagon.online/");
        }

		private void Guthub_Click(object sender, RoutedEventArgs e)
		{
            Process.Start(@"https://github.com/OneCellDM/WpfTetris");
        }
	}
}