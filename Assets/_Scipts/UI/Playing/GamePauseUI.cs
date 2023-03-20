using System;
using Kitchen.Scene;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class GamePauseUI : MonoBehaviour
    {
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button optionsButton;

        private void Awake()
        {
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
            SceneLoader.Load("MainMenuScene", "LoadingScene");
        }

        private void Start()
        {
            GameManager.Instance.stateMachine.onStateChange += _OnGameStateChange;
            Hide();
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

        private void OnDestroy()
        {
            try
            {
                GameManager.Instance.stateMachine.onStateChange -= _OnGameStateChange;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}