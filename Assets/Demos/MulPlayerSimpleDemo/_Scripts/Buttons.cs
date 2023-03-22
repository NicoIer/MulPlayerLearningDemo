using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace MulPlayerGame
{
    public class Buttons : MonoBehaviour
    {
        private Button hostButton;
        private Button serverButton;
        private Button clientButton;

        private void Awake()
        {
            hostButton = transform.Find("HostButton").GetComponent<Button>();
            serverButton = transform.Find("ServerButton").GetComponent<Button>();
            clientButton = transform.Find("ClientButton").GetComponent<Button>();
        }

        private void Start()
        {
            hostButton.onClick.AddListener(HostBtnClicked);
            serverButton.onClick.AddListener(ServerBtnClicked);
            clientButton.onClick.AddListener(ClientBtnClicked);
        }

        private void ClientBtnClicked()
        {
            NetworkManager.Singleton.StartClient();
        }

        private void ServerBtnClicked()
        {
            NetworkManager.Singleton.StartServer();
        }

        private void HostBtnClicked()
        {
            NetworkManager.Singleton.StartHost();
        }
        
        
    }
}