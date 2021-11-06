using System.Windows;
using System.Windows.Controls;

namespace WpfTetris.Pages
{
    /// <summary>
    ///     Логика взаимодействия для Pause.xaml
    /// </summary>
    public partial class Pause : UserControl
    {
        public delegate void PauseDelegate();


        public Pause()
        {
            InitializeComponent();
        }

        public event PauseDelegate ContinueEvent;
        public event PauseDelegate OpenHelpEvent;
        public event PauseDelegate ExitEvent;

        private void BackToGameButton_Click(object sender, RoutedEventArgs e)
        {
            ContinueEvent?.Invoke();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            OpenHelpEvent?.Invoke();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            ExitEvent?.Invoke();
        }
    }
}