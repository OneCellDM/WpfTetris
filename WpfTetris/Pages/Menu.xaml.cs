using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfTetris.Enums;
using WpfTetris.Models;

namespace WpfTetris.Pages
{
	/// <summary>
	///     Логика взаимодействия для Menu.xaml
	/// </summary>
	public partial class Menu : UserControl
    {
        public delegate void OpenPage();

        public delegate void SetGame(Level level);

        public Menu()
        {
            InitializeComponent();
        }

        public event SetGame SetGameEvent;
        public event OpenPage OpenScoreListEvent;
        public event OpenPage OpenCreateYouGameEvent;

        private void StandartGame(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(new Level
            {
                BackgroundType = BackgroundType.Image,
                Background = @"Backgrounds/MainTheme.jpg",
                IsAudio = true,
                Audio = @"Audios/MainTheme.mp3"
            });
        }

        private void AcapellaGame(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(new Level
            {
                BackgroundType = BackgroundType.Video,
                Background = @"Backgrounds/AcapellaAndBass1.mp4",
                IsAudio = true,
                Audio = @"Audios/Acapella.mp3"
            });
        }

        private void Bass1Game(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(new Level
            {
                BackgroundType = BackgroundType.Video,
                Background = @"Backgrounds/AcapellaAndBass1.mp4",
                IsAudio = true,
                Audio = @"Audios/Bass1.mp3"
            });
        }

        private void Bass2Game(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(new Level
            {
                BackgroundType = BackgroundType.Video,
                Background = @"Backgrounds/Bass2.mp4",
                IsAudio = true,
                Audio = @"Audios/Bass2.mp3"
            });
        }

        private void EgyptGame(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(new Level
            {
                BackgroundType = BackgroundType.Image,
                Background = @"Backgrounds/Egypt.jpg",
                IsAudio = true,
                Audio = @"Audios/Egypt.mp3"
            });
        }

        private void BitGame(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(new Level
            {
                BackgroundType = BackgroundType.Video,
                Background = @"Backgrounds/bit.mp4",
                IsAudio = true,
                Audio = @"Audios/bit.mp3",
                Colors = new[]
                {
                    Brushes.Pink,
                    Brushes.DeepPink,
                    Brushes.Yellow,
                    Brushes.AliceBlue,
                    Brushes.HotPink,
                    Brushes.LightBlue
                }
            });
        }

        private void SSSRGame(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(new Level
            {
                BackgroundType = BackgroundType.Image,
                Background = @"Backgrounds/SSSR.jpg",
                IsAudio = true,
                Audio = @"Audios/SSSR.mp3",
                Colors = new[]
                {
                    Brushes.White,
                    Brushes.Red
                }
            });
        }

        private void RuGame(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(new Level
            {
                BackgroundType = BackgroundType.Image,
                Background = @"Backgrounds/Ru.jpg",
                IsAudio = true,
                Audio = @"Audios/Ru.mp3",
                Colors = new[]
                {
                    Brushes.White,
                    Brushes.Blue,
                    Brushes.Red
                }
            });
        }

        private void HelloweenGame(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(new Level
            {
                BackgroundType = BackgroundType.Image,
                Background = @"Backgrounds/Helloween.jpg",
                IsAudio = true,
                Audio = @"Audios/Helloween.mp3",
                Colors = new[]
                {
                    Brushes.Orange,
                    Brushes.OrangeRed,
                    Brushes.DarkOrange,
                    Brushes.DarkRed
                }
            });
        }

        private void NewYearGame(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(new Level
            {
                BackgroundType = BackgroundType.Image,
                Background = @"Backgrounds/NewYear.jpg",
                IsAudio = true,
                Audio = @"Audios/NewYear.mp3",
                Colors = new[]
                {
                    Brushes.Salmon,
                    Brushes.Yellow,
                    Brushes.Red,
                    Brushes.White
                }
            });
        }

        private void TematicMenuButton_Click(object sender, RoutedEventArgs e)
        {
            TematicMenu.Visibility = Visibility.Visible;
            MainMenu.Visibility = Visibility.Collapsed;
            ExitToMainMenuButton.Visibility = Visibility.Visible;
        }

        private void ExitToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            ExitToMainMenuButton.Visibility = Visibility.Collapsed;
            TematicMenu.Visibility = Visibility.Collapsed;
            MainMenu.Visibility = Visibility.Visible;
        }

        private void RecordTable_Click(object sender, RoutedEventArgs e)
        {
            OpenScoreListEvent?.Invoke();
        }

        private void CreateYouGameButton_Click(object sender, RoutedEventArgs e)
        {
            OpenCreateYouGameEvent?.Invoke();
        }

        private void AccountPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}