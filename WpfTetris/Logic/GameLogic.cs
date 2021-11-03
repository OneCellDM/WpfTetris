using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfTetris.Enums;
using WpfTetris.Models;
using WpfTetris.Pages;

namespace WpfTetris.Logic
{
    public partial class GameLogic : IDisposable
    {
        private const int Scale = 15;
        public static Game Game;

        private readonly Dictionary<int, int> _GameScores = new Dictionary<int, int>
        {
            {1, 100}, {2, 400}, {3, 800}, {4, 1200}
        };

        private readonly Dictionary<int, int> _TimeFromLevels = new Dictionary<int, int>
        {
            {1, 500}, {2, 450}, {3, 350}, {4, 200}, {5, 150}
        };

        private MediaPlayer _AudioPlayer;
        public Level _Gamelevel;


        private int _Level = 1;
        private int _Line;
        public int _Score;
        private int _Time;

        private bool Check;
        private readonly bool Isvideo;
        private readonly Random random;

        public GameLogic(Game game, Level level)
        {
            _Gamelevel = level;
            Game = game;
            Game.Dispatcher.Invoke(() => { Game.DrawCanvas.Children.Clear(); });
            DownTimer.Stop();
            TimeTimer.Stop();
            DrawTimer.Stop();
            random = new Random(DateTime.Now.Millisecond);

            if (level.Colors != null)
                GameColors.SetColors(level.Colors);

            else GameColors.SetDefaultColors();

            Game.VideoElement.Visibility = Visibility.Collapsed;
            try
            {
                if (level.BackgroundType == BackgroundType.Image)
                    Game.Background =
                        new ImageBrush(new BitmapImage(new Uri(level.Background, UriKind.RelativeOrAbsolute)));

                if (level.BackgroundType == BackgroundType.Color)
                    Game.Background = new SolidColorBrush((Color)
                        ColorConverter.ConvertFromString(level.Background));

                if (level.BackgroundType == BackgroundType.Video)
                {
                    Game.VideoElement.Source = new Uri(level.Background, UriKind.RelativeOrAbsolute);
                    Game.VideoElement.Visibility = Visibility.Visible;
                    Isvideo = true;
                }
            }
            catch (Exception)
            {
                Game.Background = Brushes.Black;
            }

            if (level.Field != null)
                _Field = level.Field;
            else
                _Field = new int[FieldWidth, FieldHeight];


            if (level.Shape != null)
                _Shape = level.Shape;
            else
                _Shape = RandomShape();

            Level = 1;
            Score = 0;
            Time = 0;
            Line = 0;
        }

        private int Level
        {
            get => _Level;
            set
            {
                _Level = value;
                Game.Dispatcher.Invoke(() => { Game.LevelText.Text = $"Уровень: {_Level}"; });
            }
        }

        private int Line
        {
            get => _Line;
            set
            {
                _Line = value;

                Game.Dispatcher.Invoke(() => { Game.LineText.Text = $"Линия: {_Line}"; });
            }
        }

        private int Score
        {
            get => _Score;
            set
            {
                _Score = value;

                Game.Dispatcher.Invoke(() => { Game.ScoreText.Text = $"Очки: {_Score}"; });
            }
        }

        private int Time
        {
            get => _Time;
            set
            {
                _Time = value;
                var time = TimeSpan.FromSeconds(_Time);
                Game.Dispatcher.Invoke(() => { Game.TimeText.Text = $"Время {time.Minutes} : {time.Seconds}"; });
            }
        }

        public void Dispose()
        {
            TimeTimer.Stop();
            TimeTimer = null;
            DrawTimer.Stop();
            DrawTimer = null;
            DownTimer.Stop();
            DownTimer = null;

            Game.DrawCanvas.Children.Clear();
            GC.Collect();
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {
            Check = true;
            ClearShape();
            var collision = Collision.None;
            switch (e.Key)
            {
                case Key.Left:
                case Key.A:
                {
                    for (var i = 0; i < 4; i++)
                    {
                        collision = CheckCollision(_Shape[i, 0], _Shape[i, 1], Direction.Left);
                        if (collision != Collision.None)
                            break;
                    }

                    if (collision == Collision.None)
                        for (var i = 0; i < 4; i++)
                            _Shape[i, 0] = _Shape[i, 0] - 1;
                }
                    break;

                case Key.Right:
                case Key.D:
                {
                    for (var i = 0; i < 4; i++)
                    {
                        collision = CheckCollision(_Shape[i, 0], _Shape[i, 1], Direction.Right);
                        if (collision != Collision.None)
                            break;
                    }

                    if (collision == Collision.None)
                        for (var i = 0; i < 4; i++)
                            _Shape[i, 0] = _Shape[i, 0] + 1;
                }
                    break;

                case Key.Down:
                case Key.S:
                {
                    for (var i = 0; i < 4; i++)
                    {
                        collision = CheckCollision(_Shape[i, 0], _Shape[i, 1], Direction.Down);
                        if (collision != Collision.None)
                            break;
                    }

                    if (collision == Collision.None)
                        for (var i = 0; i < 4; i++)
                            _Shape[i, 1] = _Shape[i, 1] + 1;
                }
                    break;

                case Key.Up:
                case Key.W:
                    Rotate();
                    break;
            }

            Check = false;
        }

        public void Activated(object sender, EventArgs e)
        {
            DownTimer?.Stop();
            DrawTimer?.Stop();
            TimeTimer?.Stop();

            DownTimer?.Start();
            DrawTimer?.Start();
            TimeTimer?.Start();
        }

        public void StartLevel()
        {
            AreaInit();


            _color = GameColors.GetRandomColorIndex();
            Fill();


            TimeTimer.Elapsed += TimeTimer_Elapsed;

            DownTimer.Elapsed += DownTimer_Elapsed;
            TimeTimer.Interval = 1000;
            DownTimer.Interval = 500;
            DrawTimer.Interval = 20;

            DrawTimer.Elapsed += DrawTimer_Elapsed;


            DownTimer?.Start();
            DrawTimer?.Start();
            TimeTimer?.Start();

            if (_Gamelevel.IsAudio)
            {
                _AudioPlayer = new MediaPlayer();
                _AudioPlayer.Open(new Uri(_Gamelevel.Audio, UriKind.RelativeOrAbsolute));
                _AudioPlayer.Play();
                _AudioPlayer.MediaEnded += _AudioPlayer_MediaEnded;
            }

            if (Isvideo)
            {
                Game.VideoElement.LoadedBehavior = MediaState.Manual;
                Game.VideoElement.UnloadedBehavior = MediaState.Manual;
                Game.VideoElement.Volume = 0;

                Game.VideoElement.Play();
                Game.VideoElement.MediaEnded += VideoElement_MediaEnded;
            }
        }

        private void VideoElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Game.VideoElement.Position = new TimeSpan(0, 0, 0, 0);
            Game.VideoElement.Play();
        }

        private void _AudioPlayer_MediaEnded(object sender, EventArgs e)
        {
            _AudioPlayer.Position = TimeSpan.Zero;
            _AudioPlayer.Play();
        }

        public void Deactivated(object sender, EventArgs e)
        {
        }
    }
}