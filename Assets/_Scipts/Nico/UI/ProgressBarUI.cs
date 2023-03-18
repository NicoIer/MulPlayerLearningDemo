using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
    public class ProgressBarUI : MonoBehaviour
    {
        private Image _image;
        private void Awake()
        {
            _image = transform.Find("Bar").GetComponent<Image>();
            Hide();
        }

        public void SetProgress(float progress)
        {
            var target = Mathf.Clamp01(progress);
            if (target == 0 || Math.Abs(target - 1) < Mathf.Epsilon)
            {
                Hide();
                return;
            }
            Show();
            _image.fillAmount = target;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}