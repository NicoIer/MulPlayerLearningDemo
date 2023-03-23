using Unity.Netcode.Components;
using UnityEngine;

namespace Nico.DesignPattern.Singleton.Network.NetCode
{
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}