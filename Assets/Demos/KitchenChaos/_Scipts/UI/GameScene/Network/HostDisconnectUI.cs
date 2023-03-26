using System;
using Kitchen.Config;
using Kitchen.Scene;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class HostDisconnectUI : MonoBehaviour
    {
        [SerializeField] private Button playAgainButton;


        private void Start()
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += _OnClientDisconnect;
            playAgainButton.onClick.AddListener(() => { SceneLoader.Load(SceneName.MainMenuScene); });
            Hide();
        }

        private void _OnClientDisconnect(ulong clientID)
        {
            if (clientID == NetworkManager.ServerClientId)
            {
                //服务器关闭
                Show();
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void OnDestroy()
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= _OnClientDisconnect;
        }
    }
}