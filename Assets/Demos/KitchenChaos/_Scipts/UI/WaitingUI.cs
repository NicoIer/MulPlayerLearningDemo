using System;
using UnityEngine;

namespace Kitchen.UI
{
    public class WaitingUI : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.OnLocalPlayerReady += Show;//当本地玩家准备好时，显示
            GameManager.Instance.stateMachine.onStateChange += _OnStateChange;
            Hide();
        }

        private void _OnStateChange(GameState oldState, GameState newState)
        {
            //当游戏进入到 ReadyToStartState 状态时 隐藏自身 并且取消所有订阅的事件
            if (newState is ReadyToStartState)
            {//切换到ReadyToStartState状态，则显示
                Hide();
                GameManager.Instance.stateMachine.onStateChange -= _OnStateChange;
                GameManager.Instance.OnLocalPlayerReady -= Show;
                return;
            }
        }

        public void Show()
        {
            Debug.Log("show");
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            Debug.Log("hide");
            gameObject.SetActive(false);
        }
    }
}