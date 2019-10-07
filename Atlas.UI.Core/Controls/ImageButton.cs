﻿using Atlas.UI.Extensions;
using System.Windows;
using System.Windows.Media;

namespace Atlas.UI.Controls
{
    public class ImageButton : System.Windows.Controls.Button
    {
        public static readonly DependencyProperty ImageProperty = Dependency.Register<ImageSource>(nameof(Image));
        public static readonly DependencyProperty ImageWidthProperty = Dependency.Register<double>(nameof(ImageWidth));
        public static readonly DependencyProperty ImageHeightProperty = Dependency.Register<double>(nameof(ImageHeight));

        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public double ImageWidth
        {
            get => (double)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }

        public double ImageHeight
        {
            get => (double)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }

        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }
    }
}
