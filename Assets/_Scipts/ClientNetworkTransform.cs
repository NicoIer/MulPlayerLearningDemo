using Unity.Netcode.Components;
using UnityEngine;

namespace MulPlayerGame
{
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}