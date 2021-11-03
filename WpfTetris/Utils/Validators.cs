using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfTetris.Utils
{
    public static class Validators
    {
        public static (bool res, string exception) PathTextBoxValidation(string[] extensions, TextBox textBox)
        {
            textBox.BorderBrush = Brushes.Green;
            var path = textBox.Text;

            if (!File.Exists(path))
            {
                textBox.BorderBrush = Brushes.Red;
                return (false, "Файл не существует");
            }

            if (!extensions.Contains(Path.GetExtension(path)))
            {
                textBox.BorderBrush = Brushes.Red;

                var extensionsText = string.Empty;

                foreach (var extension in extensions)
                    extensionsText += extension + " ";

                return (false,
                    $"Формат файла не соотвествует поддерживаемому. Поддерживаемые форматы: {extensionsText}");
            }

            return (true, string.Empty);
        }

        public static bool BrushTextBoxValidation(TextBox textBox)
        {
            try
            {
                textBox.BorderBrush = Brushes.Green;
                var brush = (Brush) new BrushConverter().ConvertFromString(textBox.Text);
                return true;
            }
            catch (Exception)
            {
                textBox.BorderBrush = Brushes.Red;
                return false;
            }
        }
    }
}