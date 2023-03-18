using System;
using UnityEngine;

namespace Kitchen
{
    public class CuttingCounterVisual : MonoBehaviour
    {
        private CuttingCounter _cuttingCounter;
        private Animator _animator;
        private static readonly int _cutParam = Animator.StringToHash("cut");

        private void Awake()
        {
            _cuttingCounter = GetComponentInParent<CuttingCounter>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _cuttingCounter.OnCuttingEvent += _OnCuttingEvent;
        }

        private void OnDisable()
        {
            _cuttingCounter.OnCuttingEvent -= _OnCuttingEvent;
        }
        
        private void _OnCuttingEvent()
        {
            _animator.SetTrigger(_cutParam);
        }
    }
}