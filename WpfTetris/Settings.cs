using System.Windows.Media;

namespace WpfTetris
{
    public static class Settings
    {
        //Задержка падения фигуры в начале игры
        public static double StartDownDelay { get; set; }

        //конечная задержка падения фигуры
        public static double EndDownDelay { get; set; }

        //Изменение задержки при получении нового уровня
        public static double DelayChange { get; set; }

        //Количество очков для перехода на новый уровень
        public static uint NextLevelScore { get; set; }

        //цвет проэкции падения

        public static Brush ShadowColor { get; set; }

        //громкость звука

        public static double AudioVolume { get; set; }

        public static bool ShadowShow { get; set; }

        static Settings()
        {
            StartDownDelay = 500;
            EndDownDelay = 50;
            DelayChange = 25;
            NextLevelScore = 3000;
            AudioVolume = 0.5;
            ShadowColor = new SolidColorBrush(Brushes.Blue.Color) {Opacity = 0.4};
            ShadowShow = false;
        }
    }
}