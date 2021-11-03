using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfTetris.Models;



namespace WpfTetris
{
    public static class PlayerManager
    {
        private static Player _CurrentPlayer;
        public static Guid SessionUID { get; private set; }
        public static List<Player> Players = new List<Player>();
        public static Player CurrentPlayer { get; set; }
        public static string AvatarPath { get; set; }
        public static ImageSource AvatarSource
        {
            get
            {
                if (AvatarPath != null)
                    return new BitmapImage(new Uri(AvatarPath, UriKind.RelativeOrAbsolute));

                return new BitmapImage();
            }
        }
        public static string[] GetAvatas()
        {
            return new DirectoryInfo("Avatars").GetFiles().Select(x => x.FullName).ToArray();
        }
        public static void SetRandomAvatar()
        {
            var res = GetAvatas();
            AvatarPath = res[new Random().Next(res.Length)];
        }

        static PlayerManager()
        {
            SessionUID = Guid.NewGuid();
            CurrentPlayer = new Player("Неизвестный");
        }

    }
}