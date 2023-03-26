using System;
using System.Collections.Generic;
using Kitchen.Config;
using Kitchen.Manager;
using Kitchen.Scene;
using TMPro;
using Unity.Services.Lobbies.Models;
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
        [SerializeField] private GameObject lobbyBtnPrefab;
        [SerializeField] private Transform lobbyContaienr;
        List<GameObject> lobbyBtns = new List<GameObject>();

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

        private void Start()
        {
            UpdateLobbyList(null);
            LobbyManager.Instance.onLobbyListUpdate += UpdateLobbyList;
        }

        public void UpdateLobbyList(List<Lobby> lobbies)
        {
            if (lobbies == null)
                return;
            //没有足够的 则新生成
            if (lobbyBtns.Count < lobbies.Count)
            {
                for (int i = 0; i < lobbies.Count - lobbyBtns.Count; i++)
                {
                    var lobbyBtn = Instantiate(lobbyBtnPrefab, lobbyContaienr);
                    lobbyBtns.Add(lobbyBtn);
                }
            }
            //如果多余了 则隐藏 末尾的
            else if (lobbyBtns.Count > lobbies.Count)
            {
                for (int i = lobbies.Count; i < lobbyBtns.Count; i++)
                {
                    lobbyBtns[i].SetActive(false);
                }
            }
            //依次更新激活的Btn
            for (int i = 0; i < lobbies.Count; i++)
            {
                var lobbyBtn = lobbyBtns[i];
                lobbyBtn.SetActive(true);
                var lobbyBtnComp = lobbyBtn.GetComponent<LobbyButton>();
                lobbyBtnComp.SetLobby(lobbies[i]);
            }
            
        }
    }
}