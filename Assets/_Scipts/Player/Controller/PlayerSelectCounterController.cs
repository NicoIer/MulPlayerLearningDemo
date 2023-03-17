using System;
using UnityEngine;

namespace Kitchen.Player
{
    public class PlayerSelectCounterController : PlayerController
    {
        private readonly Transform _transform;
        private readonly PlayerData _data;
        public ClearCounter SelectedCounter { get; private set; }
        public event EventHandler<OnSelectedCounterChangedArgs> OnSelectedCounterChanged;
        private readonly OnSelectedCounterChangedArgs _onSelectedCounterChangedArgs = new();

        public PlayerSelectCounterController(Player player) : base(player)
        {
            _transform = player.transform;
            _data = player.data;
        }

        public override void Update()
        {
            _ShowFrontCounter();
        }

        private void _ShowFrontCounter()
        {
            if (Physics.Raycast(_transform.position, _transform.forward, out RaycastHit hit, _data.interactDistance,
                    _data.interactLayer))
            {
                if (hit.transform.TryGetComponent(out ClearCounter clearCounter))
                {
                    if (!Equals(clearCounter, SelectedCounter))
                    {
                        SelectedCounter = clearCounter;
                    }
                }
                else
                {
                    SelectedCounter = null;
                }
            }
            else
            {
                SelectedCounter = null;
            }

            _SetSelectedCounter(SelectedCounter);
        }

        private void _SetSelectedCounter(ClearCounter clearCounter)
        {
            SelectedCounter = clearCounter;
            _onSelectedCounterChangedArgs.SelectedCounter = SelectedCounter;
            OnSelectedCounterChanged?.Invoke(this, _onSelectedCounterChangedArgs);
        }
    }
}