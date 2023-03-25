﻿using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class ConnectingFailedUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI msgText;
        [SerializeField] private Button closeButton;
        private void Start()
        {
            
            closeButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
            });
            
            GameManager.Instance.OnConnecting += OnConnecting;
            GameManager.Instance.OnConnectingFailed += OnConnectingFailed;
            gameObject.SetActive(false);
        }

        private void OnConnectingFailed(ulong clientId)
        {
            msgText.text = NetworkManager.Singleton.DisconnectReason;
            gameObject.SetActive(true);
        }

        private void OnConnecting()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnConnecting -= OnConnecting;
            GameManager.Instance.OnConnectingFailed -= OnConnectingFailed;
        }
    }
}