using Atlas.UI.Enums;
using Atlas.UI.Events;
using Atlas.UI.Extensions;
using Atlas.UI.Windows.Internal;
using Atlas.UI.Windows.States;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;

namespace Atlas.UI.Windows
{
    public class Window : System.Windows.Window
    {
        public const int ShadedWindowHeight = 31;

        private Button CloseButton { get; set; }
        private Button MaximizeButton { get; set; }
        private Button MinimizeButton { get; set; }
        private Button ShadeButton { get; set; }
        private WindowChrome WindowChrome { get; set; }

        private GlowWindow Glow { get; set; }

        private double PreviousHeight { get; set; }
        private bool PreviousCanMaximize { get; set; }
        private bool PreviousCanResize { get; set; }
        private SolidColorBrush PreviousGlowBrush { get; set; }

        private Border CaptionBorder { get; set; }
        private Border MainBorder { get; set; }
        private Menu CaptionMenuControl { get; set; }

        public static readonly DependencyProperty ResizeBorderThicknessProperty = Dependency.Register<Thickness>(nameof(ResizeBorderThickness));
        public static readonly DependencyProperty ShadeStateProperty = Dependency.Register<ShadeState>(nameof(ShadeState));
        public static readonly DependencyProperty CaptionMenuProperty = Dependency.Register<ObservableCollection<MenuItem>>(nameof(CaptionMenu));
        public static readonly DependencyProperty ShowCaptionBorderProperty = Dependency.Register<bool>(nameof(ShowCaptionBorder));
        public static readonly DependencyProperty CanMaximizeProperty = Dependency.Register<bool>(nameof(CanMaximize));
        public static readonly DependencyProperty CanShadeProperty = Dependency.Register<bool>(nameof(CanShade));
        public static readonly DependencyProperty CanResizeProperty = Dependency.Register<bool>(nameof(CanResize));
        public static readonly DependencyProperty ShowCloseButtonProperty = Dependency.Register<bool>(nameof(ShowCloseButton));
        public static readonly DependencyProperty ShowShadeButtonProperty = Dependency.Register<bool>(nameof(ShowShadeButton));
        public static readonly DependencyProperty ShowMinimizeButtonProperty = Dependency.Register<bool>(nameof(ShowMinimizeButton));
        public static readonly DependencyProperty ShowMaximizeButtonProperty = Dependency.Register<bool>(nameof(ShowMaximizeButton));
        public static readonly DependencyProperty CaptionMenuAlignmentProperty = Dependency.Register<CaptionElementAlignment>(nameof(CaptionMenuAlignment));
        public static readonly DependencyProperty CaptionTitleAlignmentProperty = Dependency.Register<CaptionElementAlignment>(nameof(CaptionTitleAlignment));
        public static readonly DependencyProperty CaptionButtonsAlignmentProperty = Dependency.Register<CaptionElementAlignment>(nameof(CaptionButtonsAlignment));
        public static readonly DependencyProperty CustomCaptionContentProperty = Dependency.Register<object>(nameof(CustomCaptionContent));
        public static readonly DependencyProperty ShowIconProperty = Dependency.Register<bool>(nameof(ShowIcon));
        public static readonly DependencyProperty UseGlowEffectProperty = Dependency.Register<bool>(nameof(UseGlowEffect));
        public static readonly DependencyProperty GlowEffectBrushProperty = Dependency.Register<SolidColorBrush>(nameof(GlowEffectBrush));

        public Thickness ResizeBorderThickness
        {
            get => (Thickness)GetValue(ResizeBorderThicknessProperty);
            set => SetValue(ResizeBorderThicknessProperty, value);
        }

        public ShadeState ShadeState
        {
            get { return (ShadeState)GetValue(ShadeStateProperty); }
            set
            {
                if (value == ShadeState.Shaded)
                {
                    PreviousHeight = Height;
                    PreviousCanMaximize = CanMaximize;
                    PreviousCanResize = CanResize;

                    Height = ShadedWindowHeight;
                    MainBorder.Height = ShadedWindowHeight;

                    this.SetBorder(false);
                    this.SetResizing(false);

                    CanMaximize = false;
                }
                else
                {
                    MainBorder.Height = double.NaN;
                    Height = PreviousHeight;

                    this.SetBorder(true);
                    this.SetResizing(true);

                    CanMaximize = PreviousCanMaximize;
                    CanResize = PreviousCanResize;
                }

                SetValue(ShadeStateProperty, value);
                ShadeStateChanged?.Invoke(this, new ShadeStateChangedEventArgs(value));
            }
        }

        public ObservableCollection<MenuItem> CaptionMenu
        {
            get => (ObservableCollection<MenuItem>)GetValue(CaptionMenuProperty);
            set => SetValue(CaptionMenuProperty, value);
        }

        public bool CanMaximize
        {
            get => (bool)GetValue(CanMaximizeProperty);
            set
            {
                this.SetMaximization(value);
                SetValue(CanMaximizeProperty, value);
            }
        }

        public bool CanResize
        {
            get => (bool)GetValue(CanResizeProperty);
            set
            {
                this.SetResizing(value);
                SetValue(CanResizeProperty, value);
            }
        }

        public bool CanShade
        {
            get => (bool)GetValue(CanShadeProperty);
            set => SetValue(CanShadeProperty, value);
        }

        public bool ShowCaptionBorder
        {
            get => (bool)GetValue(ShowCaptionBorderProperty);
            set => SetValue(ShowCaptionBorderProperty, value);
        }

        public bool ShowMaximizeButton
        {
            get => (bool)GetValue(ShowMaximizeButtonProperty);
            set => SetValue(ShowMaximizeButtonProperty, value);
        }

        public bool ShowShadeButton
        {
            get => (bool)GetValue(ShowShadeButtonProperty);
            set => SetValue(ShowShadeButtonProperty, value);
        }

        public bool ShowCloseButton
        {
            get => (bool)GetValue(ShowCloseButtonProperty);
            set => SetValue(ShowCloseButtonProperty, value);
        }

        public bool ShowMinimizeButton
        {
            get => (bool)GetValue(ShowMinimizeButtonProperty);
            set => SetValue(ShowMinimizeButtonProperty, value);
        }

        public CaptionElementAlignment CaptionMenuAlignment
        {
            get => (CaptionElementAlignment)GetValue(CaptionMenuAlignmentProperty);
            set => SetValue(CaptionMenuAlignmentProperty, value);
        }

        public CaptionElementAlignment CaptionTitleAlignment
        {
            get => (CaptionElementAlignment)GetValue(CaptionTitleAlignmentProperty);
            set => SetValue(CaptionTitleAlignmentProperty, value);
        }

        public CaptionElementAlignment CaptionButtonsAlignment
        {
            get => (CaptionElementAlignment)GetValue(CaptionButtonsAlignmentProperty);
            set => SetValue(CaptionButtonsAlignmentProperty, value);
        }

        public object CustomCaptionContent
        {
            get => GetValue(CustomCaptionContentProperty);
            set => SetValue(CustomCaptionContentProperty, value);
        }

        public bool ShowIcon
        {
            get => (bool)GetValue(ShowIconProperty);
            set => SetValue(ShowIconProperty, value);
        }

        public bool UseGlowEffect
        {
            get => (bool)GetValue(UseGlowEffectProperty);
            set
            {
                if (!value && Glow != null)
                {
                    Glow.Close();
                    Glow = null;
                }
                else if (value && Glow == null)
                {
                    Glow = new GlowWindow(this)
                    {
                        GlowBrush = GlowEffectBrush
                    };
                    Glow.Show();
                }

                SetValue(UseGlowEffectProperty, value);
            }
        }

        public SolidColorBrush GlowEffectBrush
        {
            get => (SolidColorBrush)GetValue(GlowEffectBrushProperty);
            set
            {
                SetValue(GlowEffectBrushProperty, value);

                if (UseGlowEffect)
                {
                    Glow.GlowBrush = GlowEffectBrush;
                }
            }
        }

        public event EventHandler<ShadeStateChangedEventArgs> ShadeStateChanged;

        static Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));
        }

        public Window()
        {
            CaptionMenu = new ObservableCollection<MenuItem>();
            SourceInitialized += Window_SourceInitialized;
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            this.SetMaximization(CanMaximize);
            this.SetResizing(CanResize);

            if (UseGlowEffect)
            {
                Glow = new GlowWindow(this)
                {
                    GlowBrush = GlowEffectBrush
                };
                Glow.Show();
            }

            // Doesn't work from XAML, WindowChrome has no idea of a visual tree parent
            // that's this window, hence it's set in here instead of a binding.
            WindowChrome.ResizeBorderThickness = ResizeBorderThickness;
        }

        public override void OnApplyTemplate()
        {
            WindowChrome = WindowChrome.GetWindowChrome(this);

            base.OnApplyTemplate();

            CloseButton = GetTemplateChild("PART_Close") as Button;
            MaximizeButton = GetTemplateChild("PART_Maximize") as Button;
            MinimizeButton = GetTemplateChild("PART_Minimize") as Button;
            ShadeButton = GetTemplateChild("PART_Shade") as Button;

            CaptionBorder = GetTemplateChild("PART_Caption") as Border;
            MainBorder = GetTemplateChild("PART_MainBorder") as Border;

            CaptionMenuControl = GetTemplateChild("PART_CaptionMenu") as Menu;

            if (CloseButton != null)
                CloseButton.Click += CloseButton_Click;

            if (MaximizeButton != null)
                MaximizeButton.Click += MaximizeButton_Click;

            if (MinimizeButton != null)
                MinimizeButton.Click += MinimizeButton_Click;

            if (ShadeButton != null)
                ShadeButton.Click += ShadeButton_Click;

            if (CaptionBorder != null)
            {
                CaptionBorder.MouseDown += CaptionBorder_MouseDown;
                CaptionBorder.MouseMove += CaptionBorder_MouseMove;
            }
        }

        public void SetWindowBorderColor(Color color, bool matchGlowToBorder = false)
        {
            Application.Current?.Dispatcher.Invoke(() => MainBorder.BorderBrush = new SolidColorBrush(color));

            if (matchGlowToBorder)
            {
                SetWindowGlowColor(color);
            }
        }

        public void SetWindowGlowColor(Color color)
        {
            Application.Current?.Dispatcher.Invoke(() => GlowEffectBrush = new SolidColorBrush(color));
        }

        private void CaptionBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2 && CanMaximize)
                ToggleMaximizedState();
        }

        private void CaptionBorder_MouseMove(object sender, MouseEventArgs e)
        {
            if (!CaptionBorder.IsMouseDirectlyOver)
            {
                e.Handled = true;
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (WindowState == WindowState.Maximized && !CaptionMenuControl.IsMouseOver)
                {
                    Top = e.MouseDevice.GetPosition(e.Device.Target).Y;
                    ToggleMaximizedState();
                }

                DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => OnMinimizeButtonClicked(sender, e);
        private void MaximizeButton_Click(object sender, RoutedEventArgs e) => OnMaximizeButtonClicked(sender, e);
        private void ShadeButton_Click(object sender, RoutedEventArgs e) => OnShadeButtonClicked(sender, e);
        private void CloseButton_Click(object sender, RoutedEventArgs e) => OnCloseButtonClicked(sender, e);

        private void ToggleMaximizedState()
        {
            if (WindowState == WindowState.Maximized)
            {
                CanShade = true;

                WindowState = WindowState.Normal;
                Activate();
            }
            else
            {
                CanShade = false;
                WindowState = WindowState.Maximized;
            }

        }

        private void ToggleShadedState()
        {
            if (!CanShade)
                throw new InvalidOperationException("Cannot toggle shaded state while CanShade is set to false.");

            if (ShadeState == ShadeState.Shaded)
                ShadeState = ShadeState.Unshaded;
            else
                ShadeState = ShadeState.Shaded;
        }

        protected virtual void OnCloseButtonClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected virtual void OnMaximizeButtonClicked(object sender, RoutedEventArgs e)
        {
            ToggleMaximizedState();
        }

        protected virtual void OnMinimizeButtonClicked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        protected virtual void OnShadeButtonClicked(object sender, RoutedEventArgs e)
        {
            ToggleShadedState();
        }

        protected override void OnActivated(EventArgs e)
        {
            if (UseGlowEffect)
            {
                if (PreviousGlowBrush != null)
                {
                    GlowEffectBrush = PreviousGlowBrush;
                    PreviousGlowBrush = null;
                }

                Glow.MoveBehindParent();
            }

            base.OnActivated(e);
        }

        protected override void OnDeactivated(EventArgs e)
        {
            if (UseGlowEffect)
            {
                if (PreviousGlowBrush == null)
                {
                    // Has to be handled like this, binding-based approach *would* techincally
                    // work, but it would be much more error-prone and confusing.
                    //
                    // Additionally, triggers seem to give no fucks about the glow window.
                    PreviousGlowBrush = GlowEffectBrush;
                    GlowEffectBrush = new SolidColorBrush(Color.FromRgb(0x42, 0x42, 0x45));
                }
            }

            base.OnDeactivated(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (Owner != null)
                Owner.Activate();
        }
    }
}
