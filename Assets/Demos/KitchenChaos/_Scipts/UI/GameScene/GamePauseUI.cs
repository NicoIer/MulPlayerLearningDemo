﻿using System;
using Kitchen.Scene;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class GamePauseUI : MonoBehaviour
    {
        private GameObject _uiContainer;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private OptionsUI optionsUI;

        private void Awake()
        {
            _uiContainer = transform.Find("UIContainer").gameObject;
            optionsUI = transform.Find("OptionsUI").GetComponent<OptionsUI>();
            mainMenuButton.onClick.AddListener(_OnMainMenuButtonClick);
            resumeButton.onClick.AddListener(_OnResumeButtonClick);
            optionsButton.onClick.AddListener(_OnOptionsButtonClick);
            optionsUI.onColse += _OnOptionsUIClose;
        }

        private void _OnOptionsUIClose()
        {
            optionsUI.Hide();
            Show();
        }

        private void _OnOptionsButtonClick()
        {
            Hide();
            optionsUI.Show();
        }

        private void _OnResumeButtonClick()
        {
            GameManager.Instance.PauseGame();
        }

        private void _OnMainMenuButtonClick()
        {
            Debug.Log("退出游戏,停止服务器");
            NetworkManager.Singleton.Shutdown();
            
            GameManager.Instance.ExitGame();
            SceneLoader.Load("MainMenuScene", "LoadingScene");
        }


        private void OnEnable()
        {
            GameManager.Instance.stateMachine.onStateChange += _OnGameStateChange;
            Hide();
        }

        private void OnDisable()
        {
            if (GameManager.Instance is not null)
            {
                GameManager.Instance.stateMachine.onStateChange -= _OnGameStateChange;
            }
        }

        private void _OnGameStateChange(GameState arg1, GameState arg2)
        {
            if (arg2 is PausedState)
            {
                Show();
                return;
            }

            if (arg1 is PausedState)
            {
                Hide();
                return;
            }
        }


        public void Show()
        {
            resumeButton.Select();
            _uiContainer.SetActive(true);
        }

        public void Hide()
        {
            _uiContainer.SetActive(false);
            optionsUI.Hide();
        }
    }
}