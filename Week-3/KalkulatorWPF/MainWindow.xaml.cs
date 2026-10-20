using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KalkulatorWPF
{
    public partial class MainWindow : Window
    {
        private readonly CalculatorEngine _engine;
        private double _firstValue = 0;
        private string _currentOperator = "";
        private bool _isOperatorClicked = false;
        private bool _isResultShown = false;

        public MainWindow()
        {
            InitializeComponent();
            _engine = new CalculatorEngine();
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn) ProcessNumber(btn.Content.ToString());
        }

        private void ProcessNumber(string number)
        {
            if (DisplayLabel.Text == "0" || _isOperatorClicked || _isResultShown)
            {
                DisplayLabel.Text = "";
                _isOperatorClicked = false;
                _isResultShown = false;
            }
            DisplayLabel.Text += number;
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn) ProcessOperator(btn.Content.ToString());
        }

        private void ProcessOperator(string op)
        {
            try
            {
                if (!string.IsNullOrEmpty(_currentOperator) && !_isOperatorClicked)
                {
                    ProcessEquals();
                }

                _firstValue = double.Parse(DisplayLabel.Text);
                _currentOperator = op;
                OperationLabel.Text = $"{_firstValue} {op}";
                _isOperatorClicked = true;
            }
            catch (FormatException) { }
        }

        private void Root_Click(object sender, RoutedEventArgs e)
        {
            ProcessRoot();
        }

        private void ProcessRoot()
        {
            try
            {
                double value = double.Parse(DisplayLabel.Text);
                double result = _engine.PerformUnaryOperation(value, "√");
                
                if (string.IsNullOrEmpty(_currentOperator))
                    OperationLabel.Text = $"√({value})";
                else
                    OperationLabel.Text = $"{_firstValue} {_currentOperator} √({value})";
                
                DisplayLabel.Text = result.ToString();
                _isResultShown = true;
            }
            catch (ArgumentException ex)
            {
                ShowError(ex.Message);
            }
            catch (FormatException) { }
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            ProcessEquals();
        }

        private void ProcessEquals()
        {
            if (string.IsNullOrEmpty(_currentOperator)) return;

            try
            {
                double secondValue = double.Parse(DisplayLabel.Text);
                double result = _engine.PerformOperation(_firstValue, secondValue, _currentOperator);
                
                OperationLabel.Text = $"{_firstValue} {_currentOperator} {secondValue} =";
                DisplayLabel.Text = result.ToString();
                _currentOperator = "";
                _isResultShown = true;
            }
            catch (DivideByZeroException ex)
            {
                ShowError(ex.Message);
            }
            catch (OverflowException ex)
            {
                ShowError(ex.Message);
            }
            catch (FormatException) { }
        }

        private void Clear_Click(object sender, RoutedEventArgs e) => ProcessClear();
        
        private void ProcessClear()
        {
            DisplayLabel.Text = "0";
            OperationLabel.Text = "";
            _firstValue = 0;
            _currentOperator = "";
            _isOperatorClicked = false;
            _isResultShown = false;
        }

        private void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            DisplayLabel.Text = "0";
        }

        private void ShowError(string message)
        {
            DisplayLabel.Text = "Error";
            OperationLabel.Text = message;
            _currentOperator = "";
            _isResultShown = true;
        }

        // Fitur Dukungan Keyboard Fisik di WPF
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9) ProcessNumber(((int)e.Key - (int)Key.D0).ToString());
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ProcessNumber(((int)e.Key - (int)Key.NumPad0).ToString());
            else if (e.Key == Key.Add || e.Key == Key.OemPlus) ProcessOperator("+");
            else if (e.Key == Key.Subtract || e.Key == Key.OemMinus) ProcessOperator("-");
            else if (e.Key == Key.Multiply) ProcessOperator("*");
            else if (e.Key == Key.Divide || e.Key == Key.OemQuestion) ProcessOperator("/");
            else if (e.Key == Key.D6 && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) ProcessOperator("^");
            else if (e.Key == Key.R) ProcessRoot();
            else if (e.Key == Key.Enter || e.Key == Key.Return) ProcessEquals();
            else if (e.Key == Key.Escape || e.Key == Key.C) ProcessClear();
            else if (e.Key == Key.Back && DisplayLabel.Text.Length > 0 && !_isResultShown)
            {
                DisplayLabel.Text = DisplayLabel.Text.Substring(0, DisplayLabel.Text.Length - 1);
                if (string.IsNullOrEmpty(DisplayLabel.Text)) DisplayLabel.Text = "0";
            }
        }
    }
}