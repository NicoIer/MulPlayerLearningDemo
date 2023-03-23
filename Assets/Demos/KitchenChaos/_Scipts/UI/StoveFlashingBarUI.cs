using System;
using Nico.Network.Singleton;
using UnityEngine;

namespace Kitchen.UI
{
    public class StoveFlashingBarUI : MonoBehaviour
    {
        private StoveCounter _stoveCounter;
        private ProgressBarUI _progressBarUI;
        private Animator _animator;
        private readonly int _animParamHash = Animator.StringToHash("flashing");

        private void Awake()
        {
            _stoveCounter = GetComponentInParent<StoveCounter>();
            _animator = GetComponent<Animator>();
            _progressBarUI = GetComponent<ProgressBarUI>();
        }

        private void Start()
        {
            _progressBarUI.onSetProgress += OnCookingStageChange;
        }

        private void OnDestroy()
        {
            _progressBarUI.onSetProgress += OnCookingStageChange;
        }

        private void OnCookingStageChange()
        {
            var obj = _stoveCounter.GetKitchenObj().objEnum;

            if (KitchenObjOperator.WillBeBurned(obj))
            {
                _animator.SetBool(_animParamHash, true);
            }
            else
            {
                _animator.SetBool(_animParamHash, false);
            }
        }
    }
}