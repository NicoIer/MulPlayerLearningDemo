using System;

namespace Nico.Player
{
    public class OnSelectedCounterChangedArgs : EventArgs
    {
        public BaseCounter SelectedCounter { get; set; }
    }
}