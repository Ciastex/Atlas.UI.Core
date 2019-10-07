using Atlas.UI.Windows.States;

namespace Atlas.UI.Events
{
    public class ShadeStateChangedEventArgs
    {
        public ShadeState ShadeState { get; }

        public ShadeStateChangedEventArgs(ShadeState shadeState)
        {
            ShadeState = shadeState;
        }
    }
}
