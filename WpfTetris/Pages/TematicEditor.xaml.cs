using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfTetris.Enums;
using WpfTetris.Interfaces;
using WpfTetris.Models;
using WpfTetris.Utils;
using UserControl = System.Windows.Controls.UserControl;

namespace WpfTetris.Pages
{
    public partial class TematicEditor : UserControl, ICloseControl
    {
        public delegate void StartGame(Level level);

        private readonly Level _DefaultLevel = new Level
        {
            BackgroundType = BackgroundType.Image,
            Background = @"Backgrounds/MainTheme.jpg",
            IsAudio = true,
            Audio = @"Audios/MainTheme.mp3"
        };

        private bool IsInit;

        public TematicEditor()
        {
            InitializeComponent();
            SetDefaultSettings();
        }


        private Level Level { get; set; }
        public event CloseControl CloseEvent;
        public event StartGame StartGameEvent;

        public void SetDefaultSettings()
        {
            IsInit = false;
            Level = new Level
            {
                BackgroundType = _DefaultLevel.BackgroundType,
                Background = _DefaultLevel.Background,
                IsAudio = _DefaultLevel.IsAudio,
                Audio = _DefaultLevel.Audio
            };

            BackgroundTypeCombox.SelectedIndex = (int) _DefaultLevel.BackgroundType;
            BackgroundTextBox.Text = Level.Background;

            UseAudioCheckBox.IsChecked = Level.IsAudio;
            AudioFilePathTextBox.Text = Level.Audio;
            GameArea.LevelText.Text = "Уровень 1";
            GameArea.TimeText.Text = "Время 0:25";
            GameArea.LineText.Text = "Линия 0";
            GameArea.ScoreText.Text = "Очки 0";
            IsInit = true;
            SetGameSettings();
        }

        private void SetGameSettings()
        {
            GameArea.VideoElement.Visibility = Visibility.Collapsed;
            GameArea.VideoElement.LoadedBehavior = MediaState.Manual;
            GameArea.VideoElement.Stop();
            try
            {
                if (Level.BackgroundType == BackgroundType.Image)
                    GameArea.Background =
                        new ImageBrush(new BitmapImage(new Uri(Level.Background, UriKind.RelativeOrAbsolute)));

                if (Level.BackgroundType == BackgroundType.Color)
                    GameArea.Background = new SolidColorBrush((Color)
                        ColorConverter.ConvertFromString(Level.Background));

                if (Level.BackgroundType == BackgroundType.Video)
                {
                    GameArea.VideoElement.Source = new Uri(Level.Background, UriKind.RelativeOrAbsolute);
                    GameArea.VideoElement.Play();
                    GameArea.VideoElement.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                GameArea.Background = Brushes.Black;
            }
        }

        public void UpdateLevelSettings()
        {
            if (IsInit)
            {
                ErrorTextBlock.Visibility = Visibility.Collapsed;
                ErrorTextBlock.Text = string.Empty;

                if (BackgroundTextBox is null | AudioFilePathTextBox is null)
                    return;

                AudioFilePathTextBox.BorderBrush = Brushes.Gray;
                BackgroundTextBox.BorderBrush = Brushes.Gray;

                var valideData = DataIsValide();

                if (!valideData.res)
                {
                    foreach (var info in valideData.ErrorsInfo)
                        ErrorTextBlock.Text += info + "\n";

                    ErrorTextBlock.Visibility = Visibility.Visible;
                    StartGameButton.IsEnabled = false;
                    return;
                }

                if (Level != null)
                {
                    Level.BackgroundType = (BackgroundType) BackgroundTypeCombox.SelectedIndex;
                    Level.Background = BackgroundTextBox.Text;
                    Level.IsAudio = (bool) UseAudioCheckBox.IsChecked;
                    Level.Audio = AudioFilePathTextBox.Text;
                }

                StartGameButton.IsEnabled = true;
                SetGameSettings();
            }
        }

        private (bool res, List<string> ErrorsInfo) DataIsValide()
        {
            var valide = true;
            var errorsInfo = new List<string>();
            string[] exstentions = null;

            if (BackgroundTypeCombox.SelectedIndex == 0)
            {
                if (!Validators.BrushTextBoxValidation(BackgroundTextBox))
                {
                    errorsInfo.Add("Цвет фона имеет неверный формат");
                    valide = false;
                }
            }
            else
            {
                if (BackgroundTypeCombox.SelectedIndex == 1)
                    exstentions = new[] {".png", ".jpg", ".jpeg", ".bmp"};

                else
                    exstentions = new[] {".mp4", ".avi"};


                var pathValidatorRes = Validators.PathTextBoxValidation(exstentions, BackgroundTextBox);
                if (!pathValidatorRes.res)
                {
                    errorsInfo.Add("Фон: " + pathValidatorRes.exception);
                    valide = false;
                }
            }

            if (UseAudioCheckBox.IsChecked == true)
            {
                exstentions = new[] {".mp3"};
                var PathValidatorRes = Validators.PathTextBoxValidation(exstentions, AudioFilePathTextBox);
                if (PathValidatorRes.res)
                {
                    Level.Audio = AudioFilePathTextBox.Text;
                }

                else
                {
                    errorsInfo.Add("Аудиофайл: " + PathValidatorRes.exception);
                    valide = false;
                }
            }

            return (valide, errorsInfo);
        }

        private void BackgroundTypeCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BackgroundTextBlock != null)
                switch (BackgroundTypeCombox.SelectedIndex)
                {
                    case 0:
                    {
                        BackgroundTextBlock.Text = "Цвет фона:";
                        BackgroundTextBox.Text = "Black";
                        UpdateLevelSettings();
                        break;
                    }

                    case 1:
                    {
                        BackgroundTextBlock.Text = "Путь к файлу:";
                        BackgroundTextBox.Text = _DefaultLevel.Background;

                        break;
                    }
                    case 2:
                    {
                        BackgroundTextBlock.Text = "Путь к файлу:";
                        BackgroundTextBox.Text = string.Empty;

                        break;
                    }
                }
        }

        private void BackgroundTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateLevelSettings();
        }

        private void AudioFilePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateLevelSettings();
        }

        private void BackgroundSelectButtonClick(object sender, RoutedEventArgs e)
        {
            if (BackgroundTypeCombox.SelectedIndex == 0)
            {
                var dialog = new ColorDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var color = dialog.Color;
                    BackgroundTextBox.Text = color.IsNamedColor ? color.Name : $"#{color.Name}";
                    return;
                }
            }

            if (BackgroundTypeCombox.SelectedIndex > 0)
            {
                var dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                    BackgroundTextBox.Text = dialog.FileName;
            }
        }

        private void UseAudioCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Level.IsAudio = false;
            AudioFilePathTextBox.IsEnabled = false;
            UpdateLevelSettings();
        }

        private void UseAudioCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Level.IsAudio = true;
            AudioFilePathTextBox.IsEnabled = true;
            UpdateLevelSettings();
        }

        private void AudioFileSelectButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                AudioFilePathTextBox.Text = dialog.FileName;
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            SetDefaultSettings();
            UpdateLevelSettings();
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            StartGameEvent?.Invoke(Level);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            CloseEvent?.Invoke();
        }
    }
}