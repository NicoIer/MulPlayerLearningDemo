using System;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class GameClockUI : MonoBehaviour
    {
        [SerializeField] private Image clockFillImage;
        private GameObject _uiContainer;
        private float _maxPlayingTime;


        private void Awake()
        {
            _uiContainer = transform.Find("UIContainer").gameObject;
        }

        private void OnEnable()
        {
            GameManager.Instance.stateMachine.onStateChange += _OnGameStateChange;
            _Hide();
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
            if (arg2 is PlayingState playingState)
            {
                _Show();
                playingState.OnLeftTimeChange += _OnLeftTimeChange;
                _maxPlayingTime = GameManager.Instance.setting.gameDurationSetting;
                return;
            }

            if (arg1 is PlayingState playingState1)
            {
                playingState1.OnLeftTimeChange -= _OnLeftTimeChange;
                return;
            }
        }

        private void _Hide()
        {
            _uiContainer.SetActive(false);
        }

        private void _Show()
        {
            _uiContainer.SetActive(true);
        }

        private void _OnLeftTimeChange(float leftTime)
        {
            clockFillImage.fillAmount = leftTime / _maxPlayingTime;
        }
    }
}