using System.Windows.Media;
using WpfTetris.Enums;

namespace WpfTetris.Models
{
    public class Level
    {
        public BackgroundType BackgroundType;
        public string Background { get; set; }
        public bool IsAudio { get; set; }
        public string Audio { get; set; }

        public int[,] Shape { get; set; }
        public int[,] Field { get; set; }
        public SolidColorBrush[] Colors { get; set; }
    }
}