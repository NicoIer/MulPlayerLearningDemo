using System;
using TMPro;
using UnityEngine;

namespace Kitchen.UI
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI moveUpText;
        [SerializeField] TextMeshProUGUI moveDownText;
        [SerializeField] TextMeshProUGUI moveLeftText;
        [SerializeField] TextMeshProUGUI moveRightText;
        [SerializeField] TextMeshProUGUI interactText;
        [SerializeField] TextMeshProUGUI interactAltText;
        [SerializeField] TextMeshProUGUI pauseText;

        [SerializeField] TextMeshProUGUI gamePadInteractText;
        [SerializeField] TextMeshProUGUI gamePadInteractAltText;
        [SerializeField] TextMeshProUGUI gamePadPauseText;

        private void _UpdateVisual()
        {
            var input = Player.Player.Instance.input;
            moveDownText.text = input.GetBingingName(InputEnum.MoveUp);
            moveUpText.text = input.GetBingingName(InputEnum.MoveDown);
            moveLeftText.text = input.GetBingingName(InputEnum.MoveLeft);
            moveRightText.text = input.GetBingingName(InputEnum.MoveRight);
            interactText.text = input.GetBingingName(InputEnum.Interact);
            interactAltText.text = input.GetBingingName(InputEnum.InteractAlternate);
            pauseText.text = input.GetBingingName(InputEnum.Pause);
            gamePadInteractText.text = input.GetBingingName(InputEnum.GamePadInteract);
            gamePadInteractAltText.text = input.GetBingingName(InputEnum.GamePadInteractAlternate);
            gamePadPauseText.text = input.GetBingingName(InputEnum.GamePadPause);
        }

        private void Start()
        {
            _UpdateVisual();
            Player.Player.Instance.input.OnRebinding += _OnRebinding;
            GameManager.Instance.stateMachine.onStateChange += _OnStateChange;
            Show();
        }

        private void _OnStateChange(GameState arg1, GameState arg2)
        {
            if (arg1 is WaitingToStartState)
            {
                Hide();
            }
        }

        private void OnDisable()
        {
            var gameManager = GameManager.GetInstanceUnSafe();
            if (gameManager != null) gameManager.stateMachine.onStateChange -= _OnStateChange;
        }


        // private void OnDestroy()
        // {
        //     Player.Player.GetInstanceOnDisable()!.input.OnRebinding -= _OnRebinding;
        // }

        private void _OnRebinding()
        {
            _UpdateVisual();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}