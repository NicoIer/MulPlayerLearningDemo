using System;

namespace Kitchen
{
    [Serializable]
    public class GameSetting
    {
        public float gameDurationSetting = 60f;
        public int readyCountDown = 3;
        public int maxPlayerCount = 4;
    }
}