using Atlas.UI.Enums;
using Atlas.UI.Events;
using Atlas.UI.Extensions;
using System;
using System.Globalization;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Atlas.UI.Controls
{
    public class DoubleUpDown : Control
    {
        public static DependencyProperty MaximumProperty = Dependency.Register<double>(nameof(Maximum));
        public static DependencyProperty MinimumProperty = Dependency.Register<double>(nameof(Minimum));
        public static DependencyProperty ValueProperty = Dependency.Register<double>(nameof(Value));
        public static DependencyProperty StepProperty = Dependency.Register<double>(nameof(Step));
        public static DependencyProperty DigitsAfterDecimalProperty = Dependency.Register<int>(nameof(DigitsAfterDecimal));

        public static DependencyProperty DisallowUserInputProperty = Dependency.Register<bool>(nameof(DisallowUserInput));
        public static DependencyProperty ValueAlignmentProperty = Dependency.Register<TextAlignment>(nameof(ValueAlignment));
        public static DependencyProperty ValueOverflowBehaviorProperty = Dependency.Register<NumberOverflowBehavior>(nameof(ValueOverflowBehavior));

        public event EventHandler<UpDownRollOverEventArgs> RolledOver;

        private System.Windows.Controls.Button _incrementButton;
        private System.Windows.Controls.Button _decrementButton;
        private TextBox _inputTextBox;

        private Timer _spinTimer;
        private Timer _buttonHoldTimer;

        private bool _countingUp;

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public double Step
        {
            get => (double)GetValue(StepProperty);
            set => SetValue(StepProperty, value);
        }

        public int DigitsAfterDecimal
        {
            get => (int)GetValue(DigitsAfterDecimalProperty);
            set => SetValue(DigitsAfterDecimalProperty, value);
        }

        public bool DisallowUserInput
        {
            get => (bool)GetValue(DisallowUserInputProperty);
            set => SetValue(DisallowUserInputProperty, value);
        }

        public TextAlignment ValueAlignment
        {
            get => (TextAlignment)GetValue(ValueAlignmentProperty);
            set => SetValue(ValueAlignmentProperty, value);
        }

        public NumberOverflowBehavior ValueOverflowBehavior
        {
            get => (NumberOverflowBehavior)GetValue(ValueOverflowBehaviorProperty);
            set => SetValue(ValueOverflowBehaviorProperty, value);
        }

        static DoubleUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleUpDown), new FrameworkPropertyMetadata(typeof(DoubleUpDown)));
        }

        public DoubleUpDown()
        {
            _spinTimer = new Timer(75);
            _buttonHoldTimer = new Timer(500);

            _spinTimer.Elapsed += _spinTimer_Elapsed;
            _buttonHoldTimer.Elapsed += _buttonHoldTimer_Elapsed;
        }

        public override void OnApplyTemplate()
        {
            _incrementButton = GetTemplateChild("PART_Up") as System.Windows.Controls.Button;
            _decrementButton = GetTemplateChild("PART_Down") as System.Windows.Controls.Button;
            _inputTextBox = GetTemplateChild("PART_InputBox") as TextBox;

            if (_incrementButton != null)
            {
                _incrementButton.PreviewMouseDown += _incrementButton_MouseDown;
                _incrementButton.PreviewMouseUp += _anyButton_MouseUp;
            }

            if (_decrementButton != null)
            {
                _decrementButton.PreviewMouseDown += _decrementButton_MouseDown;
                _decrementButton.PreviewMouseUp += _anyButton_MouseUp;
            }

            if (_inputTextBox != null)
            {
                CommandManager.AddPreviewExecutedHandler(_inputTextBox, _inputTextBox_PreviewCommandExecuted);
                _inputTextBox.TextChanged += _inputTextBox_TextChanged;
            }

            base.OnApplyTemplate();
        }

        private void _incrementButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;

            StepValue(Step);

            _countingUp = true;
            _buttonHoldTimer.Start();
        }

        private void _anyButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;

            _buttonHoldTimer.Stop();
            _spinTimer.Stop();

            _spinTimer.Interval = 75;
        }

        private void _decrementButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;

            StepValue(-Step);

            _countingUp = false;
            _buttonHoldTimer.Start();
        }

        private void _inputTextBox_PreviewCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                var text = Clipboard.GetText();

                if (string.IsNullOrEmpty(text))
                    e.Handled = true;

                if (!double.TryParse(text, NumberStyles.AllowLeadingSign & ~NumberStyles.AllowLeadingWhite & ~NumberStyles.AllowTrailingWhite, null, out _))
                    e.Handled = true;
            }
        }

        private void _inputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(_inputTextBox.Text, out int value))
            {
                if (value > Maximum)
                    _inputTextBox.Text = Maximum.ToString();
                else if (value < Minimum)
                    _inputTextBox.Text = Minimum.ToString();
            }
        }

        private void _spinTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() => StepValue(_countingUp ? Step : -Step));
        }

        private void _buttonHoldTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() => _spinTimer.Start());

            if (_spinTimer.Interval - 5 > 1)
                _spinTimer.Interval -= 5;
        }

        public void StepValue(double by)
        {
            if (Value + by > Maximum)
            {
                switch (ValueOverflowBehavior)
                {
                    case NumberOverflowBehavior.Clamp:
                        Value = Maximum;
                        break;
                    case NumberOverflowBehavior.RollOver:
                        Value = Minimum;
                        RolledOver?.Invoke(this, new UpDownRollOverEventArgs(RollOverDirection.Up));
                        break;
                }
            }
            else if (Value + by < Minimum)
            {
                switch (ValueOverflowBehavior)
                {
                    case NumberOverflowBehavior.Clamp:
                        Value = Minimum;
                        break;
                    case NumberOverflowBehavior.RollOver:
                        Value = Maximum;
                        RolledOver?.Invoke(this, new UpDownRollOverEventArgs(RollOverDirection.Down));
                        break;
                }
            }
            else Value += by;

            Value = Math.Round(Value, DigitsAfterDecimal);
        }
    }
}
