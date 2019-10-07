using System.Windows;

namespace Atlas.UI.Controls
{
    public class ListViewItem : System.Windows.Controls.ListViewItem
    {
        static ListViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListViewItem), new FrameworkPropertyMetadata(typeof(ListViewItem)));
        }
    }
}
