using Unity.Netcode.Components;
using UnityEngine;

namespace Nico.Network.Singleton.NetCode
{
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}