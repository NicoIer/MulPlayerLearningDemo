using Nico.Components;
using UnityEngine;

namespace Kitchen.UI
{
    public class StoveFlashingBarUI : MonoBehaviour
    {
        private StoveCounter _stoveCounter;
        private ProgressBar _progressBar;
        private Animator _animator;
        private readonly int _animParamHash = Animator.StringToHash("flashing");

        private void Awake()
        {
            _stoveCounter = GetComponentInParent<StoveCounter>();
            _animator = GetComponent<Animator>();
            _progressBar = GetComponent<ProgressBar>();
        }

        private void Start()
        {
            _progressBar.onSetProgress += OnCookingStageChange;
        }

        private void OnDestroy()
        {
            _progressBar.onSetProgress += OnCookingStageChange;
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