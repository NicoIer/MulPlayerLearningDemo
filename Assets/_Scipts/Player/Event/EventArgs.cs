using System;

namespace Kitchen.Player
{
    public class OnSelectedCounterChangedArgs : EventArgs
    {
        public ClearCounter SelectedCounter { get; set; }
    }
}