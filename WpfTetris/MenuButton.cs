using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfTetris
{
	public class MenuButton : Button
	{
		public DependencyProperty RectanglesSplitProperty;

		public MenuButton()
		{
			RectanglesSplitProperty = DependencyProperty.Register("RectanglesSplit", typeof(bool), typeof(MenuButton));
		}

		public bool RectanglesSplit
		{
			get { return (bool)GetValue(RectanglesSplitProperty); }
			set { SetValue(RectanglesSplitProperty, value); }
		}
	}
}