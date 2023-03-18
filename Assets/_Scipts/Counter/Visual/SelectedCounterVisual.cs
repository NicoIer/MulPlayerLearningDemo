using System;
using UnityEngine;

namespace Nico
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

        private void Start()
        {
            Player.Player.Singleton.selectCounterController.OnSelectedCounterChanged += OnSelectedCounterChanged;
        }

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