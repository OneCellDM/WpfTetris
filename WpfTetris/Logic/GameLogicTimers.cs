using System;
using System.Linq;
using System.Timers;
using System.Windows;
using WpfTetris.Enums;
using WpfTetris.Pages;
using WpfTetris.Utils;

namespace WpfTetris.Logic
{
    public partial class GameLogic
    {
        private Timer DownTimer = new Timer();
        private Timer DrawTimer = new Timer();

        private Timer TimeTimer = new Timer();
        public Menu Menu { get; set; }
        public Lose LoseWindow { get; set; }


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

                if (NextShape == null)
                    NextShape = RandomShape();
                FillShadow();

                Collision? collision = Collision.None;

                for (var i = 0; i < 4; i++)
                {
                    collision = CheckCollision(_Shape[i, 0], _Shape[i, 1], Direction.Down);
                    if (collision != Collision.None)
                        break;
                }

                if (collision == Collision.None)
                {
                    ClearShape();

                    for (var i = 0; i < 4; i++)
                        _Shape[i, 1] = _Shape[i, 1] + 1;
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
                        if (collision != Collision.None)
                            break;
                    }

                    if (collision != Collision.None)
                    {
                        Game.Dispatcher.Invoke(() =>
                        {
                            LoseWindow.Visibility = Visibility.Visible;
                            Game.Visibility = Visibility.Collapsed;
                            _AudioPlayer.Stop();
                            Game.VideoElement.Stop();
                            DownTimer.Stop();
                            DrawTimer.Stop();
                            TimeTimer.Stop();


                            TimeTimer.Elapsed -= TimeTimer_Elapsed;
                            DownTimer.Elapsed -= DownTimer_Elapsed;
                            DrawTimer.Elapsed -= DrawTimer_Elapsed;
                            LoseWindow.ExitToMenu.Click += ExitToMenu_Click;
                            SaveScore();
                        });
                        return;
                    }
                }

                Check = false;
            }
        }

        private async void SaveScore()
        {
            try
            {
                if (_Score > 0)
                {
                    string session = PlayerManager.SessionUID.ToString();
                    string nick = PlayerManager.CurrentPlayer.NickName;
                    
                    if (nick == "Неизвестный") //Делаем запись счета без имени
                        await  MysqlManager.Instance.InsertScore( null,_Score);
                    else
                    {
                       
                        PlayerManager.CurrentPlayer.AddScore(_Score);
                        int max = PlayerManager.CurrentPlayer.GetMaxScore();
                        int id = await MysqlManager.Instance.GetIdFromNameAndGuid(session,nick);
                        if (id > -1)
                            await MysqlManager.Instance.UpdateScore(id, _Score);
                        else 
                            await MysqlManager.Instance.InsertScore(_Score, session, nick);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void ExitToMenu_Click(object sender, RoutedEventArgs e)
        {
            Game.Dispatcher.Invoke(() =>
            {
                TimeTimer.Stop();

                DownTimer.Stop();

                DrawTimer.Stop();
                Menu.Visibility = Visibility.Visible;
                Game.Visibility = Visibility.Collapsed;
                LoseWindow.Visibility = Visibility.Collapsed;

                LoseWindow.ExitToMenu.Click -= ExitToMenu_Click;
            });
        }
    }
}