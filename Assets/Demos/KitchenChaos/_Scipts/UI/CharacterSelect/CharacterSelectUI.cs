using System;
using Kitchen.Config;
using Kitchen.Manager;
using Kitchen.Scene;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class CharacterSelectUI : MonoBehaviour
    {
        [SerializeField] private Button readyButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private TextMeshProUGUI lobbyNameText;
        [SerializeField] private TextMeshProUGUI lobbyCodeText;

        private void Awake()
        {
            mainMenuButton.onClick.AddListener(() =>
            {
                LobbyManager.Instance.LeaveLobby();
                NetworkManager.Singleton.Shutdown();
                SceneLoader.Load(SceneName.MainMenuScene);
            });
            readyButton.onClick.AddListener(() => { SelectManager.Instance.SetPlayerReadyServerRpc(); });
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        }

        private void Start()
        {
            var lobby = LobbyManager.Instance.joinedLobby;
            lobbyNameText.text = $"LobbyName: {lobby.Name}";
            lobbyCodeText.text = $"LobbyCode: {lobby.LobbyCode}";
        }

        private void OnClientDisconnect(ulong obj)
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
        }
    }
}