using Unity.Netcode.Components;
using UnityEngine;

namespace Nico.Network
{
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}