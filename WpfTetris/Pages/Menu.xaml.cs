using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        public Dictionary<string, Level> DictionaryLevels = new Dictionary<string, Level>
        {
            {
                "Acapella", new Level
                {
                    BackgroundType = BackgroundType.Video,
                    Background = @"Backgrounds/AcapellaAndBass1.mp4",
                    IsAudio = true,
                    Audio = @"Audios/Acapella.mp3"
                }
            },
            {
                "Bass1", new Level
                {
                    BackgroundType = BackgroundType.Video,
                    Background = @"Backgrounds/AcapellaAndBass1.mp4",
                    IsAudio = true,
                    Audio = @"Audios/Bass1.mp3"
                }
            },
            {
                "Bass2", new Level
                {
                    BackgroundType = BackgroundType.Video,
                    Background = @"Backgrounds/Bass2.mp4",
                    IsAudio = true,
                    Audio = @"Audios/Bass2.mp3"
                }
            },
            {
                "Egypt", new Level
                {
                    BackgroundType = BackgroundType.Image,
                    Background = @"Backgrounds/Egypt.jpg",
                    IsAudio = true,
                    Audio = @"Audios/Egypt.mp3"
                }
            },
            {
                "Bit", new Level
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
                }
            },
            {
                "SSSR", new Level
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
                }
            },
            {
                "Ru", new Level
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
                }
            },
            {
                "Helloween", new Level
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
                }
            },
            {
                "NewYear", new Level
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
                }
            }
        };

        public Menu()
        {
            InitializeComponent();
        }

        public event SetGame SetGameEvent;
        public event OpenPage OpenScoreListEvent;
        public event OpenPage OpenCreateYouGameEvent;
        public event OpenPage OpenSettingsEvent;
        public event OpenPage OpenInfoEvent;
        public event OpenPage OpenHelpEvent;

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
            SetGameEvent.Invoke(DictionaryLevels["Acapella"]);
        }


        private void Bass1Game(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(DictionaryLevels["Bass1"]);
        }

        private void Bass2Game(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(DictionaryLevels["Bass2"]);
        }

        private void EgyptGame(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(DictionaryLevels["Egypt"]);
        }

        private void BitGame(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(DictionaryLevels["Bit"]);
        }

        private void SSSRGame(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(DictionaryLevels["SSSR"]);
        }

        private void RuGame(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(DictionaryLevels["Ru"]);
        }

        private void HelloweenGame(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(DictionaryLevels["Helloween"]);
        }

        private void NewYearGame(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(DictionaryLevels["NewYear"]);
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

        private void AccountPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenSettingsEvent.Invoke();
        }

        private void RandomTematicGame(object sender, RoutedEventArgs e)
        {
            SetGameEvent.Invoke(DictionaryLevels.ElementAt(new Random().Next(0, DictionaryLevels.Count)).Value);
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenInfoEvent?.Invoke();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            OpenHelpEvent?.Invoke();
        }
    }
}