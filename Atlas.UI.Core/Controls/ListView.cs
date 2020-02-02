using System.Windows;

namespace Atlas.UI.Controls
{
    public class ListView : System.Windows.Controls.ListView
    {
        static ListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListView), new FrameworkPropertyMetadata(typeof(ListView)));
        }

        protected override DependencyObject GetContainerForItemOverride() => new ListViewItem();
        protected override bool IsItemItsOwnContainerOverride(object item) => item is ListViewItem;
    }
}
