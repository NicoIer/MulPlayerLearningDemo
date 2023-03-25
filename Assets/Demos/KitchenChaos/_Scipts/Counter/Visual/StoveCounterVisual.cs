using System;
using UnityEngine;

namespace Kitchen
{
    public class StoveCounterVisual : MonoBehaviour
    {
        private StoveCounter _stoveCounter;
        private GameObject _stoveGameOnObject;
        private GameObject _particleObject;

        private void Awake()
        {
            _stoveCounter = GetComponentInParent<StoveCounter>();
            _stoveGameOnObject = transform.Find("StoveOnVisual").gameObject;
            _particleObject = transform.Find("SizzlingParticles").gameObject;
        }

        private void OnEnable()
        {
            _stoveCounter.OnStartCooking += OnStartCooking;
            _stoveCounter.OnStopCooking += OnStopCooking;
        }

        private void OnDisable()
        {
            _stoveCounter.OnStartCooking -= OnStartCooking;
            _stoveCounter.OnStopCooking -= OnStopCooking;
        }

        private void OnStopCooking()
        {
            _stoveGameOnObject.SetActive(false);
            _particleObject.SetActive(false);
        }

        private void OnStartCooking()
        {
            _stoveGameOnObject.SetActive(true);
            _particleObject.SetActive(true);
        }
    }
}