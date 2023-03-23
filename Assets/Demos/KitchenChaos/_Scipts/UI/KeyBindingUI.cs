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
        [SerializeField] private Button gamepadInteractButton;
        [SerializeField] private Button gamepadInteractAltButton;
        [SerializeField] private Button gamepadPauseButton;

        public void UpdateVisual()
        {
            var input = PlayerInput.Instance;
            moveUpButton.GetComponentInChildren<TextMeshProUGUI>().text = input.GetBingingName(InputEnum.MoveUp);
            moveDownButton.GetComponentInChildren<TextMeshProUGUI>().text = input.GetBingingName(InputEnum.MoveDown);
            moveLeftButton.GetComponentInChildren<TextMeshProUGUI>().text = input.GetBingingName(InputEnum.MoveLeft);
            moveRightButton.GetComponentInChildren<TextMeshProUGUI>().text = input.GetBingingName(InputEnum.MoveRight);
            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = input.GetBingingName(InputEnum.Interact);
            interactAltButton.GetComponentInChildren<TextMeshProUGUI>().text =
                input.GetBingingName(InputEnum.InteractAlternate);
            pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = input.GetBingingName(InputEnum.Pause);
            
            gamepadInteractButton.GetComponentInChildren<TextMeshProUGUI>().text =
                input.GetBingingName(InputEnum.GamePadInteract);
            gamepadInteractAltButton.GetComponentInChildren<TextMeshProUGUI>().text =
                input.GetBingingName(InputEnum.GamePadInteractAlternate);
            gamepadPauseButton.GetComponentInChildren<TextMeshProUGUI>().text =
                input.GetBingingName(InputEnum.GamePadPause);
            

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
            
            gamepadInteractButton.onClick.AddListener(() => { _ReBinding(InputEnum.GamePadInteract); });
            gamepadInteractAltButton.onClick.AddListener(() => { _ReBinding(InputEnum.GamePadInteractAlternate); });
            gamepadPauseButton.onClick.AddListener(() => { _ReBinding(InputEnum.GamePadPause); });
        }

        private void _ReBinding(InputEnum inputEnum)
        {
            waitingForKey.SetActive(true);
            PlayerInput.Instance.Rebinding(inputEnum, () =>
            {
                waitingForKey.SetActive(false);
                UpdateVisual();
            });
        }
    }
}