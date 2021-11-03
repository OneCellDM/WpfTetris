using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace WpfTetris
{
	public static class UIAttachedProperty
	{
		public static readonly DependencyProperty SplitToRectanglesProperty = DependencyProperty.RegisterAttached(
		"SplitToRectangles", typeof(bool), typeof(Button), new PropertyMetadata(false));

		public static readonly DependencyProperty SplitToRectanglesScaleProperty = DependencyProperty.RegisterAttached(
		"SplitToRectanglesScale", typeof(double), typeof(Button), new PropertyMetadata(15.0));

		public static void SetSplitToRectanglesScale(DependencyObject element, double value)
		{
			element.SetValue(SplitToRectanglesScaleProperty, value);
		}

		public static double GetSplitToRectanglesScale(DependencyObject element)
		{
			return (double)element.GetValue(SplitToRectanglesScaleProperty);
		}

		public static void SetSplitToRectangles(DependencyObject element, bool value)
		{
			Button b = element as Button;

			element.SetValue(SplitToRectanglesProperty, value);
			element.SetValue(Button.HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch);
			element.SetValue(Button.VerticalContentAlignmentProperty, VerticalAlignment.Stretch);

			if (value)
			{
				double scale = (double)element.GetValue(SplitToRectanglesScaleProperty);
				Canvas canvas = new Canvas();
				for (double i = -2; i < (double)element.GetValue(FrameworkElement.WidthProperty) - 2; i += scale)
				{
					canvas.Children.Add(new Line()
					{
						X1 = i,
						Y1 = 0,
						X2 = i,
						Y2 = (double)element.GetValue(FrameworkElement.HeightProperty) - 5,
						Stroke = Brushes.Black,
						StrokeThickness = 1
					});
				}
				for (double i = -2; i < (double)element.GetValue(FrameworkElement.HeightProperty) - 2; i += scale)
				{
					canvas.Children.Add(new Line()
					{
						X1 = 0,
						Y1 = i,
						X2 = (double)element.GetValue(FrameworkElement.WidthProperty) - 5,
						Y2 = i,
						Stroke = Brushes.Black,
						StrokeThickness = 1
					});
				}

				var val = element.GetValue(Button.ContentProperty);
				if (val is string)
				{
					canvas.Children.Add(
					new Border()
					{
						Width = (double)element.GetValue(FrameworkElement.WidthProperty),
						Height = (double)element.GetValue(FrameworkElement.HeightProperty),
						Child = CreateText((string)val, (Brush)element.GetValue(Button.ForegroundProperty)),
					});
				}
				else if (val is StackPanel)
				{
					canvas.Children.Add((StackPanel)val);
				}
				element.SetValue(Button.ContentProperty, canvas);
			}
		}

		public static TextBlock CreateText(string text, Brush foreground) =>
			new TextBlock()
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Text = text,
				Foreground = foreground,
				Margin = new Thickness(0, -4, 0, 0),
			};

		public static bool GetSplitToRectangles(DependencyObject element)
		{
			return (bool)element.GetValue(SplitToRectanglesProperty);
		}
	}
}