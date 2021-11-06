using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WpfTetris.Interfaces;
using WpfTetris.Models;
using WpfTetris.Utils;

namespace WpfTetris.Pages
{
    /// <summary>
    ///     Логика взаимодействия для ScoresList.xaml
    /// </summary>
    public partial class ScoresList : UserControl, ICloseControl
    {
        public ScoresList()
        {
            InitializeComponent();
        }

        public event CloseControl CloseEvent;

        public void CLearList()
        {
            ScoresListview.Items.Clear();
        }

        public async void Load()
        {
            try
            {
                ScoresListview.Items.Add(new ScoreInfo
                {
                    Name = "Текущая сессия. Количество игроков:",
                    Score = PlayerManager.Players.Count
                });
                foreach (var player in PlayerManager.Players)
                foreach (var score in player.Scores)
                    Dispatcher.Invoke(() =>
                    {
                        ScoresListview.Items.Add(new ScoreInfo
                        {
                            Name = player.NickName,
                            Score = score
                        });
                    }, DispatcherPriority.Background);

                var res = (await MysqlManager.Instance.GetScores()).OrderByDescending(x => x.score);
                ScoresListview.Items.Add(new ScoreInfo
                {
                    Name = "Счет всех игроков. Количество игроков:",
                    Score = res.Count()
                });
                if (res?.Count() == 0)
                {
                }
                else
                {
                    foreach (var item in res)
                        Dispatcher.Invoke(() =>
                        {
                            ScoresListview.Items.Add(new ScoreInfo
                            {
                                Name = string.IsNullOrEmpty(item.name) ? "Неизвестный" : item.name,
                                Score = item.score
                            });
                        }, DispatcherPriority.Background);
                }
            }
            catch (Exception)
            {
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseEvent?.Invoke();
        }
    }
}