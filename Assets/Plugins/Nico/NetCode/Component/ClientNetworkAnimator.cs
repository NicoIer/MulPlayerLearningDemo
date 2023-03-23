using Unity.Netcode.Components;
using UnityEngine;

namespace Nico.NetCode
{
    public class ClientNetworkAnimator : NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}