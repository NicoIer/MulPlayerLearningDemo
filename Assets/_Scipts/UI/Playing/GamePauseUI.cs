using System;
using Kitchen.Scene;
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

        private void Awake()
        {
            _uiContainer = transform.Find("UIContainer").gameObject;
            mainMenuButton.onClick.AddListener(_OnMainMenuButtonClick);
            resumeButton.onClick.AddListener(_OnResumeButtonClick);
            optionsButton.onClick.AddListener(_OnOptionsButtonClick);
        }

        private void _OnOptionsButtonClick()
        {
        }

        private void _OnResumeButtonClick()
        {
            GameManager.Instance.PauseGame();
        }

        private void _OnMainMenuButtonClick()
        {
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
            var gameManager = GameManager.GetInstanceOnDisable();
            if (gameManager != null)
            {
                gameManager.stateMachine.onStateChange -= _OnGameStateChange;
                return;
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
            _uiContainer.SetActive(true);
        }

        public void Hide()
        {
            _uiContainer.SetActive(false);
        }
    }
}