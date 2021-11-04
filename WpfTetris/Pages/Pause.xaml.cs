using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTetris.Pages
{
    /// <summary>
    /// Логика взаимодействия для Pause.xaml
    /// </summary>
    public partial class Pause : UserControl
    {
        public delegate void PauseDelegate();

        public event PauseDelegate ContinueEvent;
        public event PauseDelegate OpenHelpEvent;
        public event PauseDelegate ExitEvent;


        public Pause()
        {
            InitializeComponent();
        }

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