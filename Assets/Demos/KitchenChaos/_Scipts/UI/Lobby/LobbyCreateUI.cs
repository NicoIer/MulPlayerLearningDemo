using System;
using Kitchen.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class LobbyCreateUI : MonoBehaviour
    {
        [SerializeField] private Button closeBtn;
        [SerializeField] private Button createPublicBtn;
        [SerializeField] private Button createPrivateBtn;
        [SerializeField] private TMP_InputField lobbyNameInput;

        private void Awake()
        {
            createPublicBtn.onClick.AddListener(() =>
            {
                LobbyManager.Instance.Create(lobbyNameInput.text, false);
            });
            
            createPrivateBtn.onClick.AddListener(() =>
            {
                LobbyManager.Instance.Create(lobbyNameInput.text, true);
            });
            
            closeBtn.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
            });
            gameObject.SetActive(false);
        }
        
        
    }
}