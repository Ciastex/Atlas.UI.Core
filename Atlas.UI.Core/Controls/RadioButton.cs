using System.Windows;

namespace Atlas.UI.Controls
{
    public class RadioButton : System.Windows.Controls.RadioButton
    {
        static RadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButton), new FrameworkPropertyMetadata(typeof(RadioButton)));
        }
    }
}
