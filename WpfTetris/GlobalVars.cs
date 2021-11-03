using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfTetris
{
    public static class PlayerManager
    {
        public static string AvatarUrl { get; set; }
        public static ImageSource AvatarSource 
        {
            get
            {
                if (AvatarUrl != null)
                    return new BitmapImage(new Uri(AvatarUrl, UriKind.RelativeOrAbsolute));
                
                return new BitmapImage();
            }
        }
   
        public static string NickName { get; set; } = "Неизвестный";
        public static string[] GetAvatas() => Directory.GetFiles("Avatars");
        public static void SetRandomAvatar()
        {
            var res = GetAvatas();
            AvatarUrl= res[new Random().Next(res.Length + 1)];
        }
       
      

    }

}
