using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfTetris.Enums;

namespace WpfTetris.Logic
{
    public partial class GameLogic
    {
        public const int FieldWidth = 10;
        public const int FieldHeight = 25;

        private static int[,] _Field = new int[FieldWidth, FieldHeight];

        private bool CheckLineToRemove(int line)
        {
            for (var x = 0; x < FieldWidth; x++)
                if (_Field[x, line] == 0)
                    return false;
            return true;
        }

        private void DownLines(int line)
        {
            for (var y = line; y >= 1; y--)
            for (var x = 0; x < FieldWidth; x++)
                if (_Field[x, y] == 0 && _Field[x, y - 1] > 0)
                {
                    _Field[x, y] = _Field[x, y - 1];
                    _Field[x, y - 1] = 0;
                }
        }

        private void RemoveLines()
        {
            var removeCount = 0;
            var removeLineMaxIndex = -1;
            for (var y = FieldHeight - 1; y >= 0; y--)
                if (CheckLineToRemove(y))
                {
                    removeLineMaxIndex = y;
                    break;
                }

            if (removeLineMaxIndex > -1)
            {
                while (CheckLineToRemove(removeLineMaxIndex))
                {
                    RemoveLine(removeLineMaxIndex);
                    DownLines(removeLineMaxIndex);
                    removeCount++;
                }

                Line += removeCount;
                Score += GetScores(removeCount);
                RemoveLines();
            }
        }

        public void RemoveLine(int line)
        {
            for (var i = 0; i < FieldWidth; i++)
                _Field[i, line] = 0;
        }

        private void FillShadow()
        {
            var err = true;

            for (var i = 0; i < 4; i++)
            {
                _ShapeShadow[i, 0] = _Shape[i, 0];
                _ShapeShadow[i, 1] = _Shape[i, 1] - 1;
            }

            while (err)
            {
                for (var i = 0; i < 4; i++)
                    _ShapeShadow[i, 1] += 1;

                for (var i = 0; i < 4; i++)
                {
                    var segmentscolls = CheckCollision(_ShapeShadow[i, 0], _ShapeShadow[i, 1], Direction.Down);
                    if (segmentscolls != Collision.None)
                    {
                        err = false;
                        break;
                    }
                }
            }
        }

        private void Fill()
        {
            for (var i = 0; i < _Segments.Count; i++)
                Game.DrawCanvas.Children.Remove(_Segments[i]);

            _Segments.Clear();

            for (var i = 0; i < 4; i++)
            {
                var brush = new SolidColorBrush(Brushes.Blue.Color);
                brush.Opacity = 0.3;
                AddSegment(_ShapeShadow[i, 0], _ShapeShadow[i, 1], brush);
            }

            for (var i = 0; i < 4; i++)
                AddSegment(_Shape[i, 0], _Shape[i, 1], GameColors.GetBrush(_color));

            for (var x = 0; x < FieldWidth; x++)
            for (var y = 0; y < FieldHeight; y++)
                if (_Field[x, y] > 0)
                    AddSegment(x, y, GameColors.GetBrush(_Field[x, y]));
        }

        private void AddSegment(int x, int y, Brush color)
        {
            _Segments.Add(new Rectangle
            {
                Width = Scale - 2,
                Height = Scale - 2,
                Fill = color,
                Margin = new Thickness(x * Scale, y * Scale, 0, 0)
            });
            Game.DrawCanvas.Children.Add(_Segments.Last());
        }

        private void AreaInit()
        {
            for (var i = 0; i < Scale * 11; i += Scale)
                Game.DrawCanvas.Children.Add(new Line
                {
                    X1 = i,
                    Y1 = 0,
                    X2 = i,
                    Y2 = Scale * 25,
                    Stroke = Brushes.WhiteSmoke,
                    StrokeThickness = 0.5
                });
            for (var i = 0; i < Scale * 26; i += Scale)
                Game.DrawCanvas.Children.Add(new Line
                {
                    X1 = 0,
                    Y1 = i,
                    X2 = Scale * 10,
                    Y2 = i,
                    Stroke = Brushes.WhiteSmoke,
                    StrokeThickness = 0.5
                });
        }
    }
}