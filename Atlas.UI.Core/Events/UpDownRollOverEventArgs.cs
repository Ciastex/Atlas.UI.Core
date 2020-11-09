using Atlas.UI.Enums;
using System;

namespace Atlas.UI.Events
{
    public class UpDownRollOverEventArgs : EventArgs
    {
        public RollOverDirection Direction { get; }

        public UpDownRollOverEventArgs(RollOverDirection direction)
        {
            Direction = direction;
        }
    }
}
