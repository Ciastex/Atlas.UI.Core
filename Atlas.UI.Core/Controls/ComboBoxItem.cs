﻿using System.Windows;

namespace Atlas.UI.Controls
{
    public class ComboBoxItem : System.Windows.Controls.ComboBoxItem
    {
        static ComboBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxItem), new FrameworkPropertyMetadata(typeof(ComboBoxItem)));
        }
    }
}
