using Unity.Netcode.Components;
using UnityEngine;

namespace Nico.Network.Singleton.NetCode
{
    public class ClientNetworkAnimator : NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}