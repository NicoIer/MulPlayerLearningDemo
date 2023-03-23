using Unity.Netcode.Components;
using UnityEngine;

namespace Nico.NetCode
{
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}