using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class TestingUI : MonoBehaviour
    {
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;

        private void Awake()
        {
            hostButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.ConnectionApprovalCallback += ConnectionApprovalCallback;
                NetworkManager.Singleton.StartHost();
                Hide();
            });
            clientButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartClient();
                Hide();
            });
        }

        /// <summary>
        /// TODO 搞懂这个方法
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="response"></param>
        private void ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest Request,
            NetworkManager.ConnectionApprovalResponse response)
        {
            if (GameManager.Instance.CurrentState is WaitingToStartState)
            {
                response.Approved = true;
                response.CreatePlayerObject = true;
            }
            else
            {
                response.Approved = false;
            }
        }


        private void Start()
        {
            GameManager.Instance.OnLocalPlayerReady += Hide;
        }


        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnLocalPlayerReady -= Hide;
        }
    }
}