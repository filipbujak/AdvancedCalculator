using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ExpressionCalculator;

namespace AdvancedCalculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Oblicz_Click(object? sender, RoutedEventArgs e)
        {
            string input = InputBox.Text ?? "";

            if (string.IsNullOrWhiteSpace(input))
            {
                ResultText.Text = "Wpisz działanie";
                return;
            }

            try
            {
                double result = MathParser.Evaluate(input);
                ResultText.Text = $"Wynik: {Math.Round(result, 10)}";
            }
            catch (Exception ex)
            {
                ResultText.Text = $"Błąd: {ex.Message}";
            }
        }
    }
}