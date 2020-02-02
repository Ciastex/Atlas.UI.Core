using System.Windows;
using System.Windows.Controls;

namespace Atlas.UI.ExampleApplication
{
    public class ListViewTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is string)
                return (container as FrameworkElement).FindResource("StringTemplate") as DataTemplate;
            else if (item is int)
                return (container as FrameworkElement).FindResource("IntTemplate") as DataTemplate;

            return null;
        }
    }
}
