using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class KeyBindingUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moveUpText;
        [SerializeField] private TextMeshProUGUI moveDownText;
        [SerializeField] private TextMeshProUGUI moveLeftText;
        [SerializeField] private TextMeshProUGUI moveRightText;
        [SerializeField] private TextMeshProUGUI interactText;
        [SerializeField] private TextMeshProUGUI interactAltText;
        [SerializeField] private TextMeshProUGUI pauseText;
        [SerializeField] private Button moveUpButton;
        [SerializeField] private Button moveDownButton;
        [SerializeField] private Button moveLeftButton;
        [SerializeField] private Button moveRightButton;
        [SerializeField] private Button interactButton;
        [SerializeField] private Button interactAltButton;
        [SerializeField] private Button pauseButton;
        [SerializeField] private GameObject waitingForKey;

        public void UpdateVisual()
        {
            var input = Player.Player.Instance.input;
            moveUpButton.GetComponentInChildren<TextMeshProUGUI>().text = input.GetBingingName(InputEnum.MoveUp);
            moveDownButton.GetComponentInChildren<TextMeshProUGUI>().text = input.GetBingingName(InputEnum.MoveDown);
            moveLeftButton.GetComponentInChildren<TextMeshProUGUI>().text = input.GetBingingName(InputEnum.MoveLeft);
            moveRightButton.GetComponentInChildren<TextMeshProUGUI>().text = input.GetBingingName(InputEnum.MoveRight);
            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = input.GetBingingName(InputEnum.Interact);
            interactAltButton.GetComponentInChildren<TextMeshProUGUI>().text =
                input.GetBingingName(InputEnum.InteractAlternate);
            pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = input.GetBingingName(InputEnum.Pause);

            moveUpText.text = "Move Up";
            moveDownText.text = "Move Down";
            moveLeftText.text = "Move Left";
            moveRightText.text = "Move Right";
            interactText.text = "Interact";
            interactAltText.text = "Interact Alt";
            pauseText.text = "Pause";
        }

        private void Start()
        {
            UpdateVisual();
            moveUpButton.onClick.AddListener(() => { _ReBinding(InputEnum.MoveUp); });
            moveDownButton.onClick.AddListener(() => { _ReBinding(InputEnum.MoveDown); });
            moveLeftButton.onClick.AddListener(() => { _ReBinding(InputEnum.MoveLeft); });
            moveRightButton.onClick.AddListener(() => { _ReBinding(InputEnum.MoveRight); });
            interactButton.onClick.AddListener(() => { _ReBinding(InputEnum.Interact); });
            interactAltButton.onClick.AddListener(() => { _ReBinding(InputEnum.InteractAlternate); });
            pauseButton.onClick.AddListener(() => { _ReBinding(InputEnum.Pause); });
        }

        private void _ReBinding(InputEnum inputEnum)
        {
            waitingForKey.SetActive(true);
            PlayerInput input = Player.Player.Instance.input;
            input.Player.Disable();
            InputAction action;
            int idx;
            switch (inputEnum)
            {
                case InputEnum.MoveUp:
                    action = input.Player.move2D;
                    idx = 1;
                    break;
                case InputEnum.MoveDown:
                    action = input.Player.move2D;
                    idx = 2;
                    break;
                case InputEnum.MoveLeft:
                    action = input.Player.move2D;
                    idx = 3;
                    break;
                case InputEnum.MoveRight:
                    action = input.Player.move2D;
                    idx = 4;
                    break;
                case InputEnum.Interact:
                    action = input.Player.Interact;
                    idx = 0;
                    break;
                case InputEnum.InteractAlternate:
                    action = input.Player.InteractAlternate;
                    idx = 0;
                    break;
                case InputEnum.Pause:
                    action = input.Player.Pause;
                    idx = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inputEnum), inputEnum, null);
            }

            action.PerformInteractiveRebinding(idx).OnComplete((callback) =>
            {
                callback.Dispose();
                input.Enable();
                waitingForKey.SetActive(false);
                UpdateVisual();
                Player.Player.Instance.input.SaveAsJson();
            }).Start();
        }
    }
}