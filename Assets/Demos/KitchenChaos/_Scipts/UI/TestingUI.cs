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
        public static bool testingBool = false;//DEBUG

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

        public void Hide()
        {
            testingBool = true;//DEBUG
            
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            testingBool = false;//DEBUG
        }
    }
}