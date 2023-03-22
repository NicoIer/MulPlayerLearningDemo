using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kitchen.Music;
using UnityEngine;

namespace Kitchen
{
    public class StoveCounterAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _cookingAudioSource;
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
            _stoveCounter.OnCookingStageChange += _OnCookingStageChange;
        }


        private bool _isPlayingWarningSound;

        private CancellationTokenSource _playingWarningSoundCts;
        private float _warningSoundInterval = 0.5f;

        private void _OnCookingStageChange(KitchenObjEnum? obj)
        {
            if (obj is null)
            {
                _playingWarningSoundCts?.Cancel();
                return;
            }
            if (KitchenObjOperator.WillBeBurned(obj.Value))
            {
                //一直播放 直到 烹饪停止
                if (!_isPlayingWarningSound)
                {
                    _PlayingWarningTileStop().Forget();
                }
            }
            else
            {
                _playingWarningSoundCts?.Cancel();
            }
        }

        private async UniTask _PlayingWarningTileStop()
        {
            _playingWarningSoundCts = new CancellationTokenSource();
            _isPlayingWarningSound = true;
            while (_playingWarningSoundCts.IsCancellationRequested == false)
            {
                //等待 interval 后继续播放   
                SoundManager.Instance.PlayWarning(transform.position);
                await UniTask.Delay(TimeSpan.FromSeconds(_warningSoundInterval),
                    cancellationToken: _playingWarningSoundCts.Token);
            }

            _isPlayingWarningSound = false;
        }

        private void OnDisable()
        {
            _stoveCounter.OnStartCooking -= _StoveCounter_OnStartCooking;
            _stoveCounter.OnStopCooking -= _StoveCounter_OnStopCooking;
            _stoveCounter.OnCookingStageChange -= _OnCookingStageChange;
        }

        private void _StoveCounter_OnStopCooking(object sender, EventArgs e)
        {
            // _playingWarningSoundCts?.Cancel();
            _cookingAudioSource.Stop();
        }

        private void _StoveCounter_OnStartCooking(object sender, EventArgs e)
        {
            // _playingWarningSoundCts?.Cancel();
            _cookingAudioSource.Play();
        }
    }
}