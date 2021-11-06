using System.Windows;
using System.Windows.Controls;
using WpfTetris.Interfaces;

namespace WpfTetris.Pages
{
    /// <summary>
    ///     Логика взаимодействия для Lose.xaml
    /// </summary>
    public partial class Lose : UserControl, ICloseControl
    {
        public Lose()
        {
            InitializeComponent();
        }

        public event CloseControl CloseEvent;

        private void ExitToMenu_Click(object sender, RoutedEventArgs e)
        {
            CloseEvent?.Invoke();
        }
    }
}