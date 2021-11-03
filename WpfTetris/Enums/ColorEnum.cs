using System;
using System.Windows.Media;

namespace WpfTetris.Enums
{
    public static class GameColors
    {
        private static SolidColorBrush[] _Brushes;

        public static int ColorsCount { get; private set; }

        public static SolidColorBrush GetBrush(int val)
        {
            return _Brushes[val - 1];
        }

        public static void SetColors(params SolidColorBrush[] colors)
        {
            _Brushes = colors;
            ColorsCount = colors.Length;
        }

        public static void SetDefaultColors()
        {
            SetColors(Brushes.Blue, Brushes.AliceBlue, Brushes.Yellow, Brushes.Orange, Brushes.Red, Brushes.Purple,
                Brushes.Green);
        }

        public static int GetRandomColorIndex()
        {
            return new Random().Next(1, ColorsCount + 1);
        }
    }
}