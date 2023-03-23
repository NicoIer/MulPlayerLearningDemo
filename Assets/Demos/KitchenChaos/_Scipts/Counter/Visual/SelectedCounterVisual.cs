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

        private void OnEnable()
        {
            if (Player.Player.LocalInstance != null)
            {
                Player.Player.LocalInstance.SelectCounterController.OnSelectedCounterChanged +=
                    OnSelectedCounterChanged;
            }
            else
            {
                Player.Player.OnAnyPlayerSpawned += _OnAnyPlayerSpawned;
            }
        }

        private void _OnAnyPlayerSpawned()
        {
            //当有玩家出生的时候 就会触发这个事件
            //出生的玩家并不一定是本地玩家 因此 这里需要判断一下 null
            if (Player.Player.LocalInstance != null)
            {
                Player.Player.LocalInstance.SelectCounterController.OnSelectedCounterChanged += OnSelectedCounterChanged;
                Player.Player.OnAnyPlayerSpawned -= _OnAnyPlayerSpawned;
            }
        }

        private void OnDisable()
        {
            //如果抛出异常 才是正常的 因为 其本身都被删除了 事件自然也被删除了
            var player = Player.Player.LocalInstance; //TODO:这里有问题
            if (player != null)
                player.SelectCounterController.OnSelectedCounterChanged -= OnSelectedCounterChanged;
        }

        private void OnSelectedCounterChanged(object sender, BaseCounter selectedCounter)
        {
            if (selectedCounter == _clearCounter)
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