using System;
using Kitchen.Config;
using Kitchen.Manager;
using Kitchen.Scene;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] public Button createGameButton;
        [SerializeField] public Button joinGameButton;
        [SerializeField] public Button mainMenuButton;
        [SerializeField] private LobbyCreateUI lobbyCreateUI;
        [SerializeField] TMP_InputField lobbyCodeInput;
        [SerializeField] Button codeJoinBtn;
        [SerializeField] private TMP_InputField playerNameInput;

        private void Awake()
        {
            createGameButton.onClick.AddListener(() => { lobbyCreateUI.gameObject.SetActive(true); });

            joinGameButton.onClick.AddListener(() => { LobbyManager.Instance.QuickJoin(); });

            mainMenuButton.onClick.AddListener(() =>
            {
                LobbyManager.Instance.LeaveLobby();
                SceneLoader.Load(SceneName.MainMenuScene, SceneName.LoadingScene);
            });

            codeJoinBtn.onClick.AddListener(() => { LobbyManager.Instance.JoinWithCode(lobbyCodeInput.text); });
            
            playerNameInput.text = GameManager.Instance.playerName;
            playerNameInput.onValueChanged.AddListener((value) => { GameManager.Instance.playerName = value; });
        }
    }
}