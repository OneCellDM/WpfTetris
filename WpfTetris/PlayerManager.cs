using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfTetris.Models;

namespace WpfTetris
{
    public static class PlayerManager
    {
        public static List<Player> Players = new List<Player>();
        private static Player _CurrentPlayer;
        

        static PlayerManager()
        {
            SessionUID = Guid.NewGuid();
            CurrentPlayer = new Player("Неизвестный");
        }

        public static Guid SessionUID { get; }

        public static Player CurrentPlayer
        {
            get => _CurrentPlayer;
            set
            {
                _CurrentPlayer = value;
                Players.Add(_CurrentPlayer);
            }
        }

        public static string AvatarPath { get; set; }

        public static ImageSource AvatarSource =>
            AvatarPath != null
                ? new BitmapImage(new Uri(AvatarPath, UriKind.RelativeOrAbsolute))
                : new BitmapImage();

        public static string[] GetAvatars()
        {
            return new DirectoryInfo("Avatars").GetFiles().Select(x => x.FullName).ToArray();
        }

        public static void SetRandomAvatar()
        {
            var res = GetAvatars();
            AvatarPath = res[new Random().Next(res.Length)];
        }
    }
}