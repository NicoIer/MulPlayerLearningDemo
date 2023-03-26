using System;
using System.Collections.Generic;
using System.Linq;
using Kitchen.Config;
using Kitchen.Scene;
using Kitchen.Visual;
using Nico.Network;
using Unity.Netcode;
using UnityEngine;

namespace Kitchen.Manager
{
    public class SelectManager : NetSingleton<SelectManager>
    {
        public event Action onReadyChange;
        public HashSet<ulong> readyClientSet { get; private set; } = new();
        public PlayerVisual playerVisual;

        [ServerRpc(RequireOwnership = false)]
        internal void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);
            // if (readyClientSet.Contains(clientId))
            // return;
            // readyClientSet.Add(clientId);
            bool flag = NetworkManager.Singleton.ConnectedClientsIds.All(clientId => readyClientSet.Contains(clientId));
//如果所有客户端都准备好了，就进入准备就绪阶段

            if (flag)
            {
                Debug.Log($"所有玩家准备就绪,现在开始加载场景");
                //通知客户端进入游戏
                GameManager.Instance.EnterGame();
                LobbyManager.Instance.DeleteLobby();
            }
        }
        
        
        
        
        
        [ClientRpc]
        internal void SetPlayerReadyClientRpc(ulong clientId)
        {
            if (readyClientSet.Contains(clientId))
                return;
            readyClientSet.Add(clientId);
            onReadyChange?.Invoke();
        }
    }
}