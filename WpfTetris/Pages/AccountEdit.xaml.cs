using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using WpfTetris.Models;

namespace WpfTetris.Pages
{
    public partial class AccountEdit : UserControl, ICloseControl
    {

        public event CloseControl CloseEvent;
        
        public AccountEdit()
        {
            InitializeComponent();
        }
        public async Task Load()
        {
            
            NickNameTextBox.Text = PlayerManager.CurrentPlayer.NickName;
            ActiveImage.Source = PlayerManager.AvatarSource;
            ImagesList.ItemsSource = PlayerManager.GetAvatas();
        }

        private void ImagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ActiveImage.Source = new BitmapImage(new Uri(e.AddedItems[0] as string, UriKind.RelativeOrAbsolute));
            }catch(Exception ex)
            {

            }
        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e) => CloseEvent.Invoke();

        private void SaveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (NickNameTextBox.Text.Trim() != PlayerManager.CurrentPlayer.NickName)
                PlayerManager.CurrentPlayer = new Player(NickNameTextBox.Text.Trim());
            if (ImagesList.SelectedIndex>-1)
                PlayerManager.AvatarPath = ImagesList.SelectedItem as string;
            
            ImagesList.ItemsSource = null;
            GC.Collect(0);


            CloseEvent.Invoke();
        }
    }
}