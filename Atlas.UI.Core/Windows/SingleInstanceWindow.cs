using Atlas.UI.Systems;
using System;
using System.ComponentModel;

namespace Atlas.UI.Windows
{
    public abstract class SingleInstanceWindow : Window
    {
        public new void Show()
            => throw new InvalidOperationException("Use SingleInstanceWindowManager to open this type of window.");

        public new bool? ShowDialog()
            => throw new InvalidOperationException("Use SingleInstanceWindowManager to open this type of window.");

        internal void ShowSingle()
        {
            base.Show();
        }

        internal bool? ShowSingleDialog()
        {
            return base.ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (e.Cancel) return;

            SingleInstanceWindowManager.DeleteWindow(this);
            base.OnClosing(e);
        }
    }
}
