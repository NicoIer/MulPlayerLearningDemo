using System;
using UnityEngine;

namespace Kitchen.UI
{
    public class WarningIconUI : MonoBehaviour
    {
        private StoveCounter _stoveCounter;
        [SerializeField] private GameObject warningIcon;

        private void Awake()
        {
            _stoveCounter = GetComponentInParent<StoveCounter>();
            warningIcon.SetActive(false);
        }
        

        private void OnEnable()
        {
            _stoveCounter.OnCookingStageChange += OnCookingStageChange;
        }

        private void OnDisable()
        {
            _stoveCounter.OnCookingStageChange -= OnCookingStageChange;
        }

        private void OnCookingStageChange(KitchenObjEnum? obj)
        {
            //TODO 如果obj的下一个状态是烤焦 则显示WarningIcon
            //否则隐藏WarningIcon
            if (obj is null)
            {
                warningIcon.SetActive(false);
                return;
            }
            if(KitchenObjOperator.WillBeBurned(obj.Value))
            {
                warningIcon.SetActive(true);
            }
            else
            {
                warningIcon.SetActive(false);
            }
        }
    }
}