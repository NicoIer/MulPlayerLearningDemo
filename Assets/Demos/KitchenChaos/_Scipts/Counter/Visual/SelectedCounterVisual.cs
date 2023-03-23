using System;
using UnityEngine;

namespace Kitchen
{
    public class SelectedCounterVisual : MonoBehaviour
    {
        private BaseCounter _clearCounter;
        private GameObject _visualGameObj;

        private void Awake()
        {
            _clearCounter = GetComponentInParent<BaseCounter>();
            _visualGameObj = transform.Find("KitchenCounter").gameObject;
        }

        // ToDo 
        // private void OnEnable()
        // {
        //     Player.Player.Instance.selectCounterController.OnSelectedCounterChanged += OnSelectedCounterChanged;
        // }
        //
        // private void OnDisable()
        // {
        //     //如果抛出异常 才是正常的 因为 其本身都被删除了 事件自然也被删除了
        //     var player = Player.Player.GetInstanceUnSafe();
        //     if (player != null)
        //         player.selectCounterController.OnSelectedCounterChanged -= OnSelectedCounterChanged;
        // }

        private void OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedArgs e)
        {
            if (e.SelectedCounter == _clearCounter)
            {
                _Show();
            }
            else
            {
                _Hide();
            }
        }

        private void _Show()
        {
            _visualGameObj.SetActive(true);
        }

        private void _Hide()
        {
            _visualGameObj.SetActive(false);
        }
    }
}