using Unity.Netcode.Components;
using UnityEngine;

namespace Nico.Network
{
    public class ClientNetworkAnimator : NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}