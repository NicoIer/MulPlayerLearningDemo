using System;
using UnityEngine;

namespace Kitchen.UI
{
    public class ConnectingUI : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.OnConnecting += OnConnecting;
            GameManager.Instance.OnConnectingFailed += OnConnectingFailed;
            gameObject.SetActive(false);
        }

        private void OnConnectingFailed(ulong clientId)
        {
            gameObject.SetActive(false);
        }

        private void OnConnecting()
        {
            gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnConnecting -= OnConnecting;
            GameManager.Instance.OnConnectingFailed -= OnConnectingFailed;
        }
    }
}