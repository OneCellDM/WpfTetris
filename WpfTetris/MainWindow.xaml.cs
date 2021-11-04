using System;
using System.Windows;
using System.Windows.Input;
using WpfTetris.Logic;
using WpfTetris.Models;
using WpfTetris.Utils;

namespace WpfTetris
{
    public partial class MainWindow : Window
    {
        private bool Started = false;

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

            PausePage.ExitEvent += PausePage_ExitEvent;

            PausePage.OpenHelpEvent += PausePage_OpenHelpEvent;

            PausePage.ContinueEvent += PausePage_ContinueEvent;

            AccountEditPage.CloseEvent += AccountEditPage_CloseEvent;
            SettingsPage.CloseEvent += SettingsPage_CloseEvent;

            TematicEditorPage.CloseEvent += TematicEditorPage_CloseEvent;
            TematicEditorPage.StartGameEvent += SetGameEventHandler;

            ScoresPage.CloseEvent += ScoresPage_CloseEvent;

            MenuPage.OpenSettingsEvent += MenuPage_OpenSettingsEvent;
            MenuPage.OpenCreateYouGameEvent += MenuPage_OpenCreateYouGameEvent;
            MenuPage.OpenScoreListEvent += MenuPage_OpenScoreListEvent;
            MenuPage.SetGameEvent += SetGameEventHandler;

            MenuPage.AccountPanel.MouseLeftButtonDown += AccountPanel_MouseLeftButtonDown;

            PlayerManager.SetRandomAvatar();

            MenuPage.ActiveImage.Source = PlayerManager.AvatarSource;
            MenuPage.NickNameText.Text = PlayerManager.CurrentPlayer.NickName;

            OpenPage(MenuPage);
        }

        private void PausePage_ContinueEvent()
        {
            if (Game != null)
            {
                OpenPage(GamePage);
                Game.ResumeGame();
            }
        }

        private void PausePage_OpenHelpEvent()
        {
        }

        private void PausePage_ExitEvent()
        {
            if (Game != null)
            {
                Game.StopGame();
                OpenPage(MenuPage);
            }
        }

        private void SettingsPage_CloseEvent()
        {
            OpenPage(MenuPage);
        }

        private void MenuPage_OpenSettingsEvent()
        {
            OpenPage(SettingsPage);
        }

        public GameLogic Game { get; set; }

        private void AccountPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        public void OpenPage(FrameworkElement element)
        {
            LosePage.Visibility = Visibility.Collapsed;
            GamePage.Visibility = Visibility.Collapsed;
            ScoresPage.Visibility = Visibility.Collapsed;
            TematicEditorPage.Visibility = Visibility.Collapsed;
            MenuPage.Visibility = Visibility.Collapsed;
            AccountEditPage.Visibility = Visibility.Collapsed;
            SettingsPage.Visibility = Visibility.Collapsed;
            PausePage.Visibility = Visibility.Collapsed;

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
            if (Game != null)
            {
                UnSubscribeGamesEvent();
                Game.PauseEvent -= Game_PauseEvent;
            }

            Game = null;
            GC.Collect(0);

            Game = new GameLogic(GamePage, level);
            Game.PauseEvent += Game_PauseEvent;
            Game.LoseWindow = LosePage;
            Game.Menu = MenuPage;

            GamePage.NickName.Text = PlayerManager.CurrentPlayer.NickName;
            GamePage.AvatarImage.Source = PlayerManager.AvatarSource;

            OpenPage(GamePage);

            Game.StartGame();
            SubscribeGamesEvent();
        }

        private void Game_PauseEvent()
        {
            Game?.PauseGame();
            OpenPage(PausePage);
        }

        public void SubscribeGamesEvent()
        {
            Deactivated += Game.Deactivated;
            Activated += Game.Activated;
            KeyDown += Game.KeyDown;
        }

        public void UnSubscribeGamesEvent()
        {
            Deactivated -= Game.Deactivated;
            Activated -= Game.Activated;
            KeyDown -= Game.KeyDown;
        }
    }
}