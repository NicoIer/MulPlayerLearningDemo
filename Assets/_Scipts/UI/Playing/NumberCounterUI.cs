using System;
using TMPro;
using UnityEngine;

namespace Kitchen.UI
{
    //TODO 目前的单例存在很大的问题 会导致多次注册事件 或者 事件丢失
    public class NumberCounterUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        
        private void OnEnable()
        {
            GameManager.Instance.stateMachine.onStateChange += _GameManager_OnStateChange;
            GameManager.Instance.OnCountDownChange += _GameManager_OnCountDownChange;
        }

        private void _GameManager_OnStateChange(GameState oldState, GameState newState)
        {
            if (newState is ReadyToStartState)
            {
                Show();
                return;
            }

            if (newState is PlayingState)
            {
                Hide();
            }
        }


        private void _GameManager_OnCountDownChange(int num)
        {
            SetNumber(num);
        }

        public void SetNumber(int number)
        {
            textMeshProUGUI.text = number.ToString();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            var gameManager = GameManager.GetInstanceOnDisable();
            if (gameManager != null)
            {
                gameManager.stateMachine.onStateChange -= _GameManager_OnStateChange;
                gameManager.OnCountDownChange -= _GameManager_OnCountDownChange;
                return;
            }
        }
    }
}