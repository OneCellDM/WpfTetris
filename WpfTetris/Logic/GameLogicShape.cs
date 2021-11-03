using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfTetris.Enums;

namespace WpfTetris.Logic
{
    public partial class GameLogic
    {
        public static int[,] _Shape = new int[4, 2];
        public static int[,] _ShapeShadow = new int[4, 2];
        public static int[,] _NextShape;
        public int _color = 0;
        private readonly List<Rectangle> _Segments = new List<Rectangle>();

        public int[,] NextShape
        {
            get => _NextShape;
            set
            {
                _NextShape = value;
                if (_NextShape != null)
                    Game.Dispatcher.Invoke(() =>
                    {
                        Game.NextCanvas.Children.Clear();
                        for (var i = 0; i < 4; i++)
                            Game.NextCanvas.Children.Add(new Rectangle
                            {
                                Width = Scale - 2,
                                Height = Scale - 2,
                                Fill = Brushes.Red,
                                Margin = new Thickness(NextShape[i, 0] * Scale, NextShape[i, 1] * Scale, 0, 0)
                            });
                    });
            }
        }

        public int[,] RandomShape()
        {
            switch (random.Next(7))
            {
                //T
                case 0:
                    return new[,]
                    {
                        {0, 0}, {1, 0},
                        {2, 0}, {1, 1}
                    };

                //L
                case 1:
                    return new[,]
                    {
                        {0, 0}, {0, 1},
                        {0, 2}, {1, 2}
                    };

                //|
                case 2:
                    return new[,]
                    {
                        {0, 0}, {0, 1},
                        {0, 2}, {0, 3}
                    };

                //Квадрат
                case 3:
                    return new[,]
                    {
                        {0, 0}, {1, 0},
                        {0, 1}, {1, 1}
                    };

                //j
                case 4:
                    return new[,]
                    {
                        {1, 0}, {1, 1},
                        {1, 2}, {0, 2}
                    };

                //Z
                case 5:
                    return new[,]
                    {
                        {0, 0}, {1, 0},
                        {1, 1}, {2, 1}
                    };

                //S
                case 6:
                    return new[,]
                    {
                        {1, 0}, {2, 0},
                        {0, 1}, {1, 1}
                    };
            }

            return new[,]
            {
                {0, 0}, {0, 0},
                {0, 0}, {0, 0}
            };
        }

        private void ShapeFreeze()
        {
            for (var i = 0; i < 4; i++)
                _Field[_Shape[i, 0], _Shape[i, 1]] = _color;
        }

        private void Rotate()
        {
            var collision = Collision.None;
            int maxx = 0, maxy = 0;
            for (var i = 0; i < 4; i++)
            {
                if (_Shape[i, 0] > maxy)
                    maxy = _Shape[i, 0];
                if (_Shape[i, 1] > maxx)
                    maxx = _Shape[i, 1];
            }

            for (var i = 0; i < 4; i++)
            {
                var temp = _Shape[i, 0];
                var x = maxy - (maxx - _Shape[i, 1]);
                var y = maxx - (4 - (maxy - temp)) + 1;
                if (x < 0 || y < 0)
                    return;

                collision = CheckCollision(x, y, Direction.Left);

                if (collision != Collision.None)
                    break;

                collision = CheckCollision(x, y, Direction.Right);
                if (collision != Collision.None)
                    break;

                collision = CheckCollision(x, y, Direction.Down);
                if (collision != Collision.None)
                    break;
            }

            if (collision == Collision.None)
                for (var i = 0; i < 4; i++)
                {
                    var temp = _Shape[i, 0];
                    var x = maxy - (maxx - _Shape[i, 1]) - 1;
                    var y = maxx - (3 - (maxy - temp)) + 1;

                    _Shape[i, 0] = x;
                    _Shape[i, 1] = y;
                }
        }

        private void ClearShape()
        {
            for (var i = 0; i < 4; i++)
                _Field[_Shape[i, 0], _Shape[i, 1]] = 0;
        }

        private int GetScores(int count)
        {
            return _GameScores[count];
        }


        private bool CheckCollisionBounds(int x, int y, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return x - 1 == -1;

                case Direction.Right:
                    return x + 1 == FieldWidth;

                case Direction.Down:
                    return y + 1 == FieldHeight;
            }

            return false;
        }

        private Collision CheckCollision(int x, int y, Direction direction)
        {
            if (CheckCollisionBounds(x, y, direction))
                return Collision.Border;
            try
            {
                switch (direction)
                {
                    case Direction.Right:
                    {
                        if (_Field[x + 1, y] > 0)
                            return Collision.Shape;
                    }
                        break;

                    case Direction.Down:
                    {
                        if (_Field[x, y + 1] > 0)
                            return Collision.Shape;
                    }
                        break;

                    case Direction.Left:
                    {
                        if (_Field[x - 1, y] > 0)
                            return Collision.Shape;
                    }
                        break;
                }
            }
            catch (Exception ex)
            {
            }

            return Collision.None;
        }
    }
}