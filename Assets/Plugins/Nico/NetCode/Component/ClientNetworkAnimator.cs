using Unity.Netcode.Components;
using UnityEngine;

namespace Nico.DesignPattern.Singleton.Network.NetCode
{
    public class ClientNetworkAnimator : NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}