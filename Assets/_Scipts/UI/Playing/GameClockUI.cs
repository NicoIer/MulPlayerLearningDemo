using System;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class GameClockUI : MonoBehaviour
    {
        [SerializeField] private Image clockFillImage;
        private float _maxPlayingTime;

        private void Start()
        {
            GameManager.Instance.stateMachine.onStateChange += _OnGameStateChange;
            _Hide();
        }

        private void OnDestroy()
        {
            try
            {
                GameManager.Instance.stateMachine.onStateChange -= _OnGameStateChange;
            }
            catch (Exception)
            {
                //ignore
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
            gameObject.SetActive(false);
        }

        private void _Show()
        {
            gameObject.SetActive(true);
        }

        private void _OnLeftTimeChange(float leftTime)
        {
            clockFillImage.fillAmount = leftTime / _maxPlayingTime;
        }
    }
}