using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfTetris
{
	public static class BrushesEnum
	{
		public static Brush GetBrush(int val)
		{
			switch (val)
			{
				case 1:
					return Brushes.Blue;

				case 2:
					return Brushes.AliceBlue;

				case 3:
					return Brushes.Yellow;

				case 4:
					return Brushes.Orange;

				case 5:
					return Brushes.Red;

				case 6:
					return Brushes.Purple;

				case 7:
					return Brushes.Green;
			}
			return null;
		}
	}
}