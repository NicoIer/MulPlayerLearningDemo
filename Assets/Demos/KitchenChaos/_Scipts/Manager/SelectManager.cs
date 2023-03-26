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
                SceneLoader.LoadNet(SceneName.GameScene, SceneName.LoadingScene);
                LoadSceneClientRpc(SceneName.LoadingScene); //通知客户端加载LoadingScene
                GameManager.Instance.EnterGame();
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


        [ClientRpc]
        internal void LoadSceneClientRpc(string sceneName)
        {
            if (IsHost || IsServer)
            {
                return;
            }

            SceneLoader.Load(sceneName);
        }
    }
}