using System;

namespace Kitchen.Player
{
    public class OnSelectedCounterChangedArgs : EventArgs
    {
        public BaseCounter SelectedCounter { get; set; }
    }
}