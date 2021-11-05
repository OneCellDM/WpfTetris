using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfTetris.Enums;
using WpfTetris.Models;
using WpfTetris.Pages;
using WpfTetris.Utils;
using Menu = WpfTetris.Pages.Menu;

namespace WpfTetris.Logic
{
    public class GameLogic : IDisposable
    {
        public delegate void PauseDelegate();

        public event PauseDelegate PauseEvent;
        private bool Exit = false;
        private const int Scale = 15;
        public const int FieldWidth = 10;
        public const int FieldHeight = 25;
        private int currentScore = 0;

        private static int[,] _Field = new int[FieldWidth, FieldHeight];
        private static int[,] _Shape = new int[4, 2];
        private static int[,] _ShapeShadow = new int[4, 2];
        private static int[,] _NextShape;

        private readonly Dictionary<int, int> _GameScores = new Dictionary<int, int>
        {
            {1, 100}, {2, 400}, {3, 800}, {4, 1200}
        };

        private readonly List<Rectangle> _Segments = new List<Rectangle>();
        private readonly bool Isvideo;
        private readonly Random random;
        private MediaPlayer _AudioPlayer;
        private int _Level = 1;
        private int _Line;
        private int _Score;
        private int _Time;
        private int _color;
        private bool Check;
        private Timer DownTimer;
        private Timer DrawTimer;
        private Timer TimeTimer;


        public GameLogic(Game game, Level level)
        {
            _Gamelevel = level;
            Game = game;

            Game.Dispatcher.Invoke(() => { Game.DrawCanvas.Children.Clear(); });
            random = new Random(DateTime.Now.Millisecond);
            if (level.Colors != null) GameColors.SetColors(level.Colors);
            else GameColors.SetDefaultColors();
            Game.VideoElement.Visibility = Visibility.Collapsed;
            try
            {
                if (level.BackgroundType == BackgroundType.Image)
                    Game.Background =
                        new ImageBrush(new BitmapImage(new Uri(level.Background, UriKind.RelativeOrAbsolute)));
                if (level.BackgroundType == BackgroundType.Color)
                    Game.Background = new SolidColorBrush((Color) ColorConverter.ConvertFromString(level.Background));
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

            if (level.Field != null) _Field = level.Field;
            else _Field = new int[FieldWidth, FieldHeight];
            if (level.Shape != null) _Shape = level.Shape;
            else _Shape = RandomShape();
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

        private int[,] NextShape
        {
            get => _NextShape;
            set
            {
                _NextShape = value;
                if (_NextShape != null)
                    Game.Dispatcher.Invoke(() =>
                    {
                        Game.NextCanvas.Children.Clear();
                        for (var i = 0; i < 4; i++)
                            Game.NextCanvas.Children.Add(new Rectangle
                            {
                                Width = Scale - 2,
                                Height = Scale - 2,
                                Fill = Brushes.Red,
                                Margin = new Thickness(NextShape[i, 0] * Scale, NextShape[i, 1] * Scale, 0, 0)
                            });
                    });
            }
        }

        public Game Game;
        public Level _Gamelevel;
        public Menu Menu { get; set; }
        public Lose LoseWindow { get; set; }


        private void AddScore(int removeCount)
        {
            Score += _GameScores[removeCount];
            currentScore += _GameScores[removeCount];
        }

        public void Dispose()
        {
            Game.DrawCanvas.Children.Clear();
            GC.Collect();
        }

        public void MoveShape(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                {
                    for (var i = 0; i < 4; i++)
                        _Shape[i, 0] = _Shape[i, 0] - 1;

                    break;
                }
                case Direction.Right:
                {
                    for (var i = 0; i < 4; i++)
                        _Shape[i, 0] = _Shape[i, 0] + 1;

                    break;
                }
                case Direction.Down:
                {
                    for (var i = 0; i < 4; i++)
                        _Shape[i, 1] = _Shape[i, 1] + 1;

                    break;
                }
            }
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {
            Check = true;
            ClearShape();
            var collision = Collision.None;
            switch (e.Key)
            {
                case Key.Escape:
                    PauseEvent?.Invoke();
                    break;
                case Key.Left:
                case Key.A:
                {
                    for (var i = 0; i < 4; i++)
                    {
                        collision = CheckCollision(_Shape[i, 0], _Shape[i, 1], Direction.Left);
                        if (collision != Collision.None) break;
                    }

                    if (collision == Collision.None)
                        MoveShape(Direction.Left);
                }
                    break;
                case Key.Right:
                case Key.D:
                {
                    for (var i = 0; i < 4; i++)
                    {
                        collision = CheckCollision(_Shape[i, 0], _Shape[i, 1], Direction.Right);
                        if (collision != Collision.None) break;
                    }

                    if (collision == Collision.None)
                        MoveShape(Direction.Right);
                }
                    break;
                case Key.Down:
                case Key.S:
                {
                    for (var i = 0; i < 4; i++)
                    {
                        collision = CheckCollision(_Shape[i, 0], _Shape[i, 1], Direction.Down);
                        if (collision != Collision.None) break;
                    }

                    if (collision == Collision.None)
                        MoveShape(Direction.Down);
                    break;
                }

                case Key.Up:
                case Key.W:
                    Rotate();
                    break;
            }

            Check = false;
        }

        public void StartGame()
        {
            AreaInit();
            _color = GameColors.GetRandomColorIndex();
            Fill();
            InitTimers(Settings.StartDownDelay);
            SubscribeTimers();
            StartTimers();
            if (_Gamelevel.IsAudio)
            {
                _AudioPlayer = new MediaPlayer();
                _AudioPlayer.Volume = Settings.AudioVolume;
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

        public void ResumeGame()
        {
            Game.Dispatcher.Invoke(() =>
            {
                StartTimers();
                _AudioPlayer.Play();
                if (Isvideo)
                    Game.VideoElement.Play();
            });
        }

        public void PauseGame()
        {
            Game.Dispatcher.Invoke(() =>
            {
                StopTimers();

                _AudioPlayer.Pause();
                if (Isvideo)
                    Game.VideoElement.Pause();
            });
        }

        public void StopGame()
        {
            Game.Dispatcher.Invoke(() =>
            {
                StopTimers();
                UnSubscribeTimers();
                _AudioPlayer.Stop();
                if (Isvideo)
                    Game.VideoElement.Stop();
            });
        }

        private void EndGame()
        {
            StopGame();
            Game.Dispatcher.Invoke(() =>
            {
                LoseWindow.Visibility = Visibility.Visible;
                Game.Visibility = Visibility.Collapsed;
              
                LoseWindow.ExitToMenu.Click += ExitToMenu_Click;

                SaveScore();
            });
        }

        private void LoseVideoElement_MediaOpened(object sender, RoutedEventArgs e)
        {
           
        }
        public void SetVolume(double volume)
        {
            if(_AudioPlayer!=null)
                _AudioPlayer.Volume = volume;
        }

        public void InitTimers(double downElapsed)
        {
            TimeTimer = new Timer();
            DrawTimer = new Timer();
            DownTimer = new Timer();
            TimeTimer.Interval = 1000;
            DownTimer.Interval = downElapsed;
            DrawTimer.Interval = 20;
        }

        public void StartTimers()
        {
            DownTimer.Start();
            DrawTimer.Start();
            TimeTimer.Start();
        }

        public void StopTimers()
        {
            DownTimer.Stop();
            DrawTimer.Stop();
            TimeTimer.Stop();
        }

        public void SubscribeTimers()
        {
            TimeTimer.Elapsed += TimeTimer_Elapsed;
            DownTimer.Elapsed += DownTimer_Elapsed;
            DrawTimer.Elapsed += DrawTimer_Elapsed;
        }

        public void UnSubscribeTimers()
        {
            TimeTimer.Elapsed -= TimeTimer_Elapsed;
            DownTimer.Elapsed -= DownTimer_Elapsed;
            DrawTimer.Elapsed -= DrawTimer_Elapsed;
        }

        private bool CheckLineToRemove(int line)
        {
            for (var x = 0; x < FieldWidth; x++)
                if (_Field[x, line] == 0)
                    return false;
            return true;
        }

        private void DownLines(int line)
        {
            for (var y = line; y >= 1; y--)
            for (var x = 0; x < FieldWidth; x++)
                if (_Field[x, y] == 0 && _Field[x, y - 1] > 0)
                {
                    _Field[x, y] = _Field[x, y - 1];
                    _Field[x, y - 1] = 0;
                }
        }

        private void RemoveLines()
        {
            var removeCount = 0;
            var removeLineMaxIndex = -1;
            for (var y = FieldHeight - 1; y >= 0; y--)
                if (CheckLineToRemove(y))
                {
                    removeLineMaxIndex = y;
                    break;
                }

            if (removeLineMaxIndex > -1)
            {
                while (CheckLineToRemove(removeLineMaxIndex))
                {
                    RemoveLine(removeLineMaxIndex);
                    DownLines(removeLineMaxIndex);
                    removeCount++;
                }

                Line += removeCount;
                AddScore(removeCount);

                if (currentScore >= Settings.NextLevelScore)
                {
                    Level++;
                    if (DownTimer.Interval > Settings.EndDownDelay)
                    {
                        StopTimers();
                        DownTimer.Interval -= Settings.DelayChange;
                        StartTimers();
                        currentScore = 0;
                    }
                }

                RemoveLines();
            }
        }

        public void RemoveLine(int line)
        {
            for (var i = 0; i < FieldWidth; i++)
                _Field[i, line] = 0;
        }

        private void ShadowCalculatePosition()
        {
            var err = true;
            for (var i = 0; i < 4; i++)
            {
                _ShapeShadow[i, 0] = _Shape[i, 0];
                _ShapeShadow[i, 1] = _Shape[i, 1] - 1;
            }

            while (err)
            {
                for (var i = 0; i < 4; i++) _ShapeShadow[i, 1] += 1;
                for (var i = 0; i < 4; i++)
                {
                    var segmentscolls = CheckCollision(_ShapeShadow[i, 0], _ShapeShadow[i, 1], Direction.Down);
                    if (segmentscolls != Collision.None)
                    {
                        err = false;
                        break;
                    }
                }
            }
        }

        private void Fill()
        {
            for (var i = 0; i < _Segments.Count; i++) Game.DrawCanvas.Children.Remove(_Segments[i]);
            _Segments.Clear();
            if (Settings.ShadowShow)
                for (var i = 0; i < 4; i++)
                    AddSegment(_ShapeShadow[i, 0], _ShapeShadow[i, 1], Settings.ShadowColor);

            for (var i = 0; i < 4; i++) AddSegment(_Shape[i, 0], _Shape[i, 1], GameColors.GetBrush(_color));
            for (var x = 0; x < FieldWidth; x++)
            for (var y = 0; y < FieldHeight; y++)
                if (_Field[x, y] > 0)
                    AddSegment(x, y, GameColors.GetBrush(_Field[x, y]));
        }

        private void AddSegment(int x, int y, Brush color)
        {
            _Segments.Add(new Rectangle
            {
                Width = Scale - 2,
                Height = Scale - 2,
                Fill = color,
                Margin = new Thickness(x * Scale, y * Scale, 0, 0)
            });
            Game.DrawCanvas.Children.Add(_Segments.Last());
        }

        private void AreaInit()
        {
            for (var i = 0; i < Scale * 11; i += Scale)
                Game.DrawCanvas.Children.Add(new Line
                {
                    X1 = i,
                    Y1 = 0,
                    X2 = i,
                    Y2 = Scale * 25,
                    Stroke = Brushes.WhiteSmoke,
                    StrokeThickness = 0.5
                });
            for (var i = 0; i < Scale * 26; i += Scale)
                Game.DrawCanvas.Children.Add(new Line
                {
                    X1 = 0,
                    Y1 = i,
                    X2 = Scale * 10,
                    Y2 = i,
                    Stroke = Brushes.WhiteSmoke,
                    StrokeThickness = 0.5
                });
        }

        public int[,] RandomShape()
        {
            switch (random.Next(7))
            {
                //T
                case 0: return new[,] {{0, 0}, {1, 0}, {2, 0}, {1, 1}};

                //L
                case 1: return new[,] {{0, 0}, {0, 1}, {0, 2}, {1, 2}};

                //|
                case 2: return new[,] {{0, 0}, {0, 1}, {0, 2}, {0, 3}};

                //Квадрат
                case 3: return new[,] {{0, 0}, {1, 0}, {0, 1}, {1, 1}};

                //j
                case 4: return new[,] {{1, 0}, {1, 1}, {1, 2}, {0, 2}};

                //Z
                case 5: return new[,] {{0, 0}, {1, 0}, {1, 1}, {2, 1}};

                //S
                case 6: return new[,] {{1, 0}, {2, 0}, {0, 1}, {1, 1}};
            }

            return new[,] {{0, 0}, {0, 0}, {0, 0}, {0, 0}};
        }

        private void ShapeFreeze()
        {
            for (var i = 0; i < 4; i++)
                _Field[_Shape[i, 0], _Shape[i, 1]] = _color;
        }

        private void Rotate()
        {
            var collision = Collision.None;
            int maxx = 0, maxy = 0;
            for (var i = 0; i < 4; i++)
            {
                if (_Shape[i, 0] > maxy) maxy = _Shape[i, 0];
                if (_Shape[i, 1] > maxx) maxx = _Shape[i, 1];
            }

            for (var i = 0; i < 4; i++)
            {
                var temp = _Shape[i, 0];
                var x = maxy - (maxx - _Shape[i, 1]) + 1;
                var y = maxx - (4 - (maxy - temp)) + 1;
                if (x < 0 || y < 0) return;
                collision = CheckCollision(x, y, Direction.Left);
                if (collision != Collision.None)
                    break;
                collision = CheckCollision(x, y, Direction.Down);
                if (collision != Collision.None) break;
            }

            if (collision == Collision.None)
            {
                for (var i = 0; i < 4; i++)
                {
                    var temp = _Shape[i, 0];
                    var x = maxy - (maxx - _Shape[i, 1]);
                    var y = maxx - (4 - (maxy - temp)) + 1;
                    _Shape[i, 0] = x;
                    _Shape[i, 1] = y;
                }
                MoveShape(Direction.Down);
            }
        }

        private void ClearShape()
        {
            try
            {
                for (var i = 0; i < 4; i++)
                    _Field[_Shape[i, 0], _Shape[i, 1]] = 0;
            }
            catch (Exception EX)
            {
            }
        }


        private bool CheckCollisionBounds(int x, int y, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left: return x - 1 == -1;
                case Direction.Right: return x + 1 == FieldWidth;
                case Direction.Down: return y + 1 == FieldHeight;
            }

            return false;
        }

        private Collision CheckCollision(int x, int y, Direction direction)
        {
            if (CheckCollisionBounds(x, y, direction)) return Collision.Border;
            try
            {
                switch (direction)
                {
                    case Direction.Right:
                    {
                        if (_Field[x + 1, y] > 0) return Collision.Shape;
                    }
                        break;
                    case Direction.Down:
                    {
                        if (_Field[x, y + 1] > 0) return Collision.Shape;
                    }
                        break;
                    case Direction.Left:
                    {
                        if (_Field[x - 1, y] > 0) return Collision.Shape;
                    }
                        break;
                }
            }
            catch (Exception ex)
            {
            }

            return Collision.None;
        }

        private void DrawTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Game.Dispatcher.Invoke(() => { Fill(); });
            }
            catch (Exception EX)
            {
            }
        }

        private void TimeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Time++;
        }

        private void DownTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!Check)
            {
                Check = true;
                if (NextShape == null) NextShape = RandomShape();

                if (Settings.ShadowShow)
                    ShadowCalculatePosition();

                Collision? collision = Collision.None;
                for (var i = 0; i < 4; i++)
                {
                    collision = CheckCollision(_Shape[i, 0], _Shape[i, 1], Direction.Down);
                    if (collision != Collision.None) break;
                }

                if (collision == Collision.None)
                {
                    ClearShape();
                    MoveShape(Direction.Down);
                }
                else
                {
                    ShapeFreeze();
                    RemoveLines();
                    _color = GameColors.GetRandomColorIndex();
                    _Shape = NextShape;
                    NextShape = null;
                    for (var i = 0; i < 4; i++)
                    {
                        collision = CheckCollision(_Shape[i, 0], _Shape[i, 1], Direction.Down);
                        if (collision != Collision.None) break;
                    }

                    if (collision != Collision.None)
                    {
                        EndGame();
                        return;
                    }
                }

                Check = false;
            }
        }

        //TODO: перенести этот метод в MainWindow
        private void ExitToMenu_Click(object sender, RoutedEventArgs e)
        {
            Game.Dispatcher.Invoke(() =>
            {
                Exit = true;
             
                Menu.Visibility = Visibility.Visible;
                Game.Visibility = Visibility.Collapsed;
                LoseWindow.Visibility = Visibility.Collapsed;
                LoseWindow.ExitToMenu.Click -= ExitToMenu_Click;
                Exit = false;
            });
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

        private async void SaveScore()
        {
            try
            {
                PlayerManager.CurrentPlayer.AddScore(_Score);
                if (_Score > 0)
                {
                    var session = PlayerManager.SessionUID.ToString();
                    var nick = PlayerManager.CurrentPlayer.NickName;
                    if (nick == "Неизвестный")
                    {
                        await MysqlManager.Instance.InsertScore(null, _Score);
                    }
                    else
                    {
                       
                        var max = PlayerManager.CurrentPlayer.GetMaxScore();

                        var id = await MysqlManager.Instance.GetIdFromNameAndGuid(session, nick);

                        if (id > -1) await MysqlManager.Instance.UpdateScore(id, _Score);

                        else await MysqlManager.Instance.InsertScore(_Score, session, nick);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void Activated(object sender, EventArgs e)
        {
            ResumeGame();
        }

        public void Deactivated(object sender, EventArgs e)
        {
            PauseGame();
            
        }
    }
}