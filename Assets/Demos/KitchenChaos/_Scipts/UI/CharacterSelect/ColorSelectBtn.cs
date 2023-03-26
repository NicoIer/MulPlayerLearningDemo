using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class ColorSelectBtn : MonoBehaviour
    {
        public static int idx = 0;
        [SerializeField] private Button button;
        [SerializeField] private int colorId;
        [SerializeField] private Image image;
        [FormerlySerializedAs("selctedIcon")] [SerializeField] private GameObject selectedIcon;

        private void Start()
        {
            image.color = GameManager.Instance.GetColor(colorId);
            button.onClick.AddListener(() =>
            {
                var config = GameManager.Instance.GetLocalPlayerConfig();
                config.colorId = colorId;
                GameManager.Instance.SePlayerColor(colorId);
            });
            GameManager.Instance.playerConfigs.OnListChanged += _OnPlayerConfigListChange;
            
            UpdateVisual();
        }

        private void _OnPlayerConfigListChange(NetworkListEvent<PlayerConfig> changeevent)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            var config = GameManager.Instance.GetLocalPlayerConfig();

            if (config.colorId == colorId)
            {
                selectedIcon.SetActive(true);
            }
            else
            {
                selectedIcon.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            GameManager.Instance.playerConfigs.OnListChanged -= _OnPlayerConfigListChange;
            colorId = 0;
        }
        
    }
}