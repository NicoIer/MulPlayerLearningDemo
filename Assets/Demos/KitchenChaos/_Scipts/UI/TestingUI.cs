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
                NetworkManager.Singleton.StartHost();
                Hide();
            });
            clientButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartClient();
                Hide();
            });
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