using System;
using UnityEngine;

namespace Kitchen
{
    public class StoveCounterAudio : MonoBehaviour
    {
        private AudioSource _audioSource;
        private StoveCounter _stoveCounter;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _stoveCounter = GetComponentInParent<StoveCounter>();
        }

        private void OnEnable()
        {
            _stoveCounter.OnStartCooking += _StoveCounter_OnStartCooking;
            _stoveCounter.OnStopCooking += _StoveCounter_OnStopCooking;
        }
        private void OnDisable()
        {
            _stoveCounter.OnStartCooking -= _StoveCounter_OnStartCooking;
            _stoveCounter.OnStopCooking -= _StoveCounter_OnStopCooking;
        }

        private void _StoveCounter_OnStopCooking(object sender, EventArgs e)
        {
            _audioSource.Stop();
        }

        private void _StoveCounter_OnStartCooking(object sender, EventArgs e)
        {
            _audioSource.Play();
        }


    }
}