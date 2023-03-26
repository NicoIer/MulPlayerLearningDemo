using System;
using Kitchen.Manager;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class LobbyConnetingUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI msgText;
        [SerializeField] private Button closeButton;

        private void Start()
        {
            closeButton.onClick.AddListener(() => { gameObject.SetActive(false); });

            GameManager.Instance.OnConnecting += OnConnecting;
            GameManager.Instance.OnConnectingFailed += OnConnectingFailed;
            LobbyManager.Instance.OnStartCreate += OnCreateLobby;
            LobbyManager.Instance.OnCreateFailed += OnCreateFailed;
            LobbyManager.Instance.onJoinStart += OnJoinStart;
            LobbyManager.Instance.onJoinFailed += OnJoinFailed;
            LobbyManager.Instance.onCodeJoinFailed += OnCodeJoinFailed;

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnConnecting -= OnConnecting;
            GameManager.Instance.OnConnectingFailed -= OnConnectingFailed;
            LobbyManager.Instance.OnStartCreate -= OnCreateLobby;
            LobbyManager.Instance.OnCreateFailed -= OnCreateFailed;
            LobbyManager.Instance.onJoinStart -= OnJoinStart;
            LobbyManager.Instance.onJoinFailed -= OnJoinFailed;
            LobbyManager.Instance.onCodeJoinFailed -= OnCodeJoinFailed;
            
        }

        private void OnCodeJoinFailed()
        {
            ShowMessage("Code Join Failed");
        }

        private void OnJoinFailed()
        {
            ShowMessage("Quick Join Failed");
        }

        private void OnJoinStart()
        {
            ShowMessage("Joining....");
        }

        private void OnCreateFailed()
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
        }

        private void OnCreateLobby()
        {
            ShowMessage("Creating.... Lobby");
        }

        private void ShowMessage(string msg)
        {
            msgText.text = msg;
            if (string.IsNullOrEmpty(msg))
            {
                msgText.text = "Failed to Connect";
            }

            gameObject.SetActive(true);
        }


        private void OnConnectingFailed(ulong clientId)
        {
            msgText.text = NetworkManager.Singleton.DisconnectReason;
            if (string.IsNullOrEmpty(msgText.text))
            {
                msgText.text = "Failed to Connect";
            }

            gameObject.SetActive(true);
        }

        private void OnConnecting()
        {
            gameObject.SetActive(false);
        }
    }
}