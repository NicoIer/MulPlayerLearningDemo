using System;
using Kitchen.Music;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class OptionsUI : MonoBehaviour
    {
        [SerializeField] private Button soundButton;
        [SerializeField] private Button musicButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI soundText;
        [SerializeField] private TextMeshProUGUI musicText;

        private void Awake()
        {
            soundButton.onClick.AddListener(() => { });
            musicButton.onClick.AddListener(() => { });
            closeButton.onClick.AddListener(() => { });
        }

        public void Hide()
        {
        }

        public void Show()
        {
        }
    }
}