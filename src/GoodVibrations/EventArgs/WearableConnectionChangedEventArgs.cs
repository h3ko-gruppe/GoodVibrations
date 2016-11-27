using System;
using GoodVibrations.Enums;

namespace GoodVibrations.EventArgs
{
    public class WearableConnectionChangedEventArgs : System.EventArgs
    {
        public ConnectionState State
        {
            get;
        }

        public WearableConnectionChangedEventArgs(ConnectionState state)
        {
            State = state;
        }
    }
}
