using System;
using UnityEngine;

namespace Nico.Player
{
    public class PlayerSelectCounterController : PlayerController
    {
        private readonly Transform _transform;
        private readonly PlayerData _data;
        public BaseCounter SelectedCounter { get; private set; }
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
                if (hit.transform.TryGetComponent(out BaseCounter clearCounter))
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

        private void _SetSelectedCounter(BaseCounter clearCounter)
        {
            SelectedCounter = clearCounter;
            _onSelectedCounterChangedArgs.SelectedCounter = SelectedCounter;
            OnSelectedCounterChanged?.Invoke(this, _onSelectedCounterChangedArgs);
        }
    }
}