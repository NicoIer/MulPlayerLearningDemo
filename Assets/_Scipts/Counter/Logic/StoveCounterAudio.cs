using System;
using Cysharp.Threading.Tasks;
using Kitchen.Music;
using UnityEngine;

namespace Kitchen
{
    public class StoveCounterAudio : MonoBehaviour
    {
        [SerializeField]private AudioSource _cookingAudioSource;
        private StoveCounter _stoveCounter;

        private void Awake()
        {
            _cookingAudioSource = GetComponent<AudioSource>();
            _stoveCounter = GetComponentInParent<StoveCounter>();
        }

        private void OnEnable()
        {
            _stoveCounter.OnStartCooking += _StoveCounter_OnStartCooking;
            _stoveCounter.OnStopCooking += _StoveCounter_OnStopCooking;
            _stoveCounter.onCookingStageChange += _OnCookingStageChange;
        }

        private void _OnCookingStageChange(KitchenObjEnum obj)
        {
            if (KitchenObjOperator.WillBeBurned(obj))
            {
                //一直播放 直到 烹饪停止
                SoundManager.Instance.PlayWarning(transform.position);
                // _PlayingWarningTileStop().Forget();
               
            }
        }

        private async UniTask _PlayingWarningTileStop()
        {
            
        }

        private void OnDisable()
        {
            _stoveCounter.OnStartCooking -= _StoveCounter_OnStartCooking;
            _stoveCounter.OnStopCooking -= _StoveCounter_OnStopCooking;
            _stoveCounter.onCookingStageChange -= _OnCookingStageChange;
        }

        private void _StoveCounter_OnStopCooking(object sender, EventArgs e)
        {
            _cookingAudioSource.Stop();
        }

        private void _StoveCounter_OnStartCooking(object sender, EventArgs e)
        {
            _cookingAudioSource.Play();
        }
    }
}