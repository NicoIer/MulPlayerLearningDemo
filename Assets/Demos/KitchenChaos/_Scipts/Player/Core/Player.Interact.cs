using System;
using UnityEngine;

namespace Kitchen.Player
{
    public partial class Player
    {
        public event Action pause; 
        private void OnPerformInteract()
        {
            if (!GameManager.Instance.IsPlaying()) return;
            
            //这是多播委托的调用 其实没必要通知每个Counter
            if (SelectedCounter == null)
                return;
            Debug.Log("按下交互键");
            SelectedCounter.Interact(this);
        }

        private void OnPerformInteractAlternate()
        {
            if (!GameManager.Instance.IsPlaying()) return;
            
            if (SelectedCounter == null) return;
            if (SelectedCounter.TryGetComponent(out IInteractAlternate interactAlternate))
            {
                interactAlternate.InteractAlternate(this);
            }
        }
    }
}