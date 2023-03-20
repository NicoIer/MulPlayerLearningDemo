using System;
using TMPro;
using UnityEngine;

namespace Kitchen.UI
{
    //TODO 目前的单例存在很大的问题 会导致多次注册事件 或者 事件丢失
    public class NumberCounterUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        private bool _listening = false;

        private void OnEnable()
        {
            try
            {
                GameManager.Instance.OnCountDownChange += _GameManager_OnCountDownChange;
                GameManager.Instance.OnGameReadyToStart += _GameManager_OnGameReadyToStart;
                _listening = true;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void Start()
        {
            Hide();
            if (!_listening)
            {
                
                GameManager.Instance.OnCountDownChange += _GameManager_OnCountDownChange;
                GameManager.Instance.OnGameReadyToStart += _GameManager_OnGameReadyToStart;
                GameManager.Instance.OnStartPlaying += _GameManager_OnStartPlaying;
            }
        }

        private void _GameManager_OnStartPlaying()
        {
            Hide();
        }

        private void OnDisable()
        {
            GameManager.Instance.OnCountDownChange -= _GameManager_OnCountDownChange;
            GameManager.Instance.OnGameReadyToStart -= _GameManager_OnGameReadyToStart;
            _listening = false;
        }

        private void _GameManager_OnGameReadyToStart()
        {
            Show();
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
            textMeshProUGUI.enabled = true;
        }

        public void Hide()
        {
            textMeshProUGUI.enabled = false;
        }
    }
}