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
        private GameObject _uiContainer;
        public event Action onColse;
        
        private void Start()
        {
            _uiContainer = transform.Find("UIContainer").gameObject;
            soundButton.onClick.AddListener(() =>
            {
                var volume = SoundManager.Instance.GetVolume();
                volume += 0.1f;
                SoundManager.Instance.ChangeVolume(volume);
                _UpdateText();
            });
            musicButton.onClick.AddListener(() =>
            {
                var volume = MusicManager.Instance.GetVolume();
                volume += 0.1f;
                MusicManager.Instance.ChangeVolume(volume);
                _UpdateText();
            });
            closeButton.onClick.AddListener(() =>
            {
                Hide();
                onColse?.Invoke();
            });
            Hide();
        }

        public void Hide()
        {
            _uiContainer.SetActive(false);
        }

        public void Show()
        {
            _uiContainer.SetActive(true);
            _UpdateText();
        }

        private void _UpdateText()
        {
            
            soundText.text = $"Sound: {SoundManager.Instance.GetVolume():0.0}";
            musicText.text = $"Music: {MusicManager.Instance.GetVolume():0.0}";
        }
    }
}