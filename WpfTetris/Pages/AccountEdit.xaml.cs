using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfTetris.Interfaces;
using WpfTetris.Models;

namespace WpfTetris.Pages
{
    public partial class AccountEdit : UserControl, ICloseControl
    {
        public AccountEdit()
        {
            InitializeComponent();
        }

        public event CloseControl CloseEvent;

        public async Task Load()
        {
            NickNameTextBox.Text = PlayerManager.CurrentPlayer.NickName;
            ActiveImage.Source = PlayerManager.AvatarSource;
            ImagesList.ItemsSource = PlayerManager.GetAvatars();
        }

        private void ImagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ActiveImage.Source = new BitmapImage(new Uri(e.AddedItems[0] as string, UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseEvent.Invoke();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (NickNameTextBox.Text.Trim() != PlayerManager.CurrentPlayer.NickName)
                PlayerManager.CurrentPlayer = new Player(NickNameTextBox.Text.Trim());
            if (ImagesList.SelectedIndex > -1)
                PlayerManager.AvatarPath = ImagesList.SelectedItem as string;

            ImagesList.ItemsSource = null;
            GC.Collect(0);


            CloseEvent.Invoke();
        }
    }
}