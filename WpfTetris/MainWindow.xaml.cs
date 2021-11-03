using System;
using System.Windows;
using WpfTetris.Logic;
using WpfTetris.Models;
using WpfTetris.Utils;

namespace WpfTetris
{
    public partial class MainWindow : Window
    {
        private bool f;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                MysqlManager.CreateInstanceFromFile(MysqlManager.DbInfoFile);
            }
            catch (Exception ex)
            {
            }

            AccountEditPage.CloseEvent += AccountEditPage_CloseEvent;

            TematicEditorPage.CloseEvent += TematicEditorPage_CloseEvent;
            TematicEditorPage.StartGameEvent += SetGameEventHandler;
            ScoresPage.CloseEvent += ScoresPage_CloseEvent;

            MenuPage.OpenCreateYouGameEvent += MenuPage_OpenCreateYouGameEvent;
            MenuPage.OpenScoreListEvent += MenuPage_OpenScoreListEvent;
            MenuPage.SetGameEvent += SetGameEventHandler;

            MenuPage.AccountPanel.MouseLeftButtonDown += AccountPanel_MouseLeftButtonDown;

            PlayerManager.SetRandomAvatar();

            MenuPage.ActiveImage.Source = PlayerManager.AvatarSource;
            MenuPage.NickNameText.Text = PlayerManager.CurrentPlayer.NickName;

            OpenPage(MenuPage);
        }

        private void AccountPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AccountEditPage.Load();
            OpenPage(AccountEditPage);
        }

        private void AccountEditPage_CloseEvent()
        {
            MenuPage.ActiveImage.Source = PlayerManager.AvatarSource;
            MenuPage.NickNameText.Text = PlayerManager.CurrentPlayer.NickName;

            OpenPage(MenuPage);
        }

        public GameLogic Game { get; set; }

        public void OpenPage(FrameworkElement element)
        {
            LosePage.Visibility = Visibility.Collapsed;
            GamePage.Visibility = Visibility.Collapsed;
            ScoresPage.Visibility = Visibility.Collapsed;
            TematicEditorPage.Visibility = Visibility.Collapsed;
            MenuPage.Visibility = Visibility.Collapsed;
            AccountEditPage.Visibility = Visibility.Collapsed;

            element.Visibility = Visibility.Visible;
        }

        private void ScoresPage_CloseEvent()
        {
            ScoresPage.CLearList();
            OpenPage(MenuPage);
        }

        private void TematicEditorPage_CloseEvent()
        {
            OpenPage(MenuPage);
        }

        private void MenuPage_OpenCreateYouGameEvent()
        {
            TematicEditorPage.SetDefaultSettings();
            OpenPage(TematicEditorPage);
        }

        private void MenuPage_OpenScoreListEvent()
        {
            Dispatcher.Invoke(() =>
            {
                ScoresPage.CLearList();
                ScoresPage.Visibility = Visibility.Visible;
            });
            ScoresPage.Load();
        }

        private void SetGameEventHandler(Level level)
        {
            if (f)
            {
                Deactivated -= Game.Deactivated;
                Activated -= Game.Activated;
                KeyDown -= Game.KeyDown;
            }

            Game = null;
            GC.Collect(0);

            Game = new GameLogic(GamePage, level);

            Game.LoseWindow = LosePage;
            Game.Menu = MenuPage;
            Deactivated += Game.Deactivated;
            Activated += Game.Activated;
            KeyDown += Game.KeyDown;

            GamePage.NickName.Text = PlayerManager.CurrentPlayer.NickName;
            GamePage.AvatarImage.Source = PlayerManager.AvatarSource;


            OpenPage(GamePage);

            Game.StartLevel();
            f = true;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}