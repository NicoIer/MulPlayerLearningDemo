using System.Collections.Generic;
using System.Linq;
using Kitchen.Config;
using Kitchen.Scene;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class CharacterSelectUI : NetworkBehaviour
    {
        [SerializeField] private Button readyButton;

        private HashSet<ulong> readyClientSet = new();

        private void Awake()
        {
            readyButton.onClick.AddListener(() => { SetPlayerReadyServerRpc(); });
        }

        //TODO 拆分逻辑
        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            if (readyClientSet.Contains(serverRpcParams.Receive.SenderClientId))
                return;
            readyClientSet.Add(serverRpcParams.Receive.SenderClientId);
            bool flag = NetworkManager.Singleton.ConnectedClientsIds.All(clientId => readyClientSet.Contains(clientId));
//如果所有客户端都准备好了，就进入准备就绪阶段
            if (flag)
            {
                SceneLoader.LoadNet(SceneName.GameScene, SceneName.LoadingScene);
                LoadSceneClientRpc(SceneName.LoadingScene); //通知客户端加载LoadingScene
                GameManager.Instance.EnterGame();
            }
        }

        //TODO 拆分逻辑
        [ClientRpc]
        private void LoadSceneClientRpc(string sceneName)
        {
            if (IsHost || IsServer)
            {
                return;
            }

            SceneLoader.Load(sceneName);
        }
    }
}