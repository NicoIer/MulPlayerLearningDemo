using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Nico
{
    public class ProgressBarUI : MonoBehaviour
    {
        private Image _image;
        private void Awake()
        {
            _image = transform.Find("Bar").GetComponent<Image>();
            _Hide();
        }

        public void SetProgress(float progress)
        {
            var target = Mathf.Clamp01(progress);
            if (target == 0 || Math.Abs(target - 1) < Mathf.Epsilon)
            {
                _Hide();
                return;
            }
            _Show();
            _image.fillAmount = target;
        }

        private void _Hide()
        {
            gameObject.SetActive(false);
        }

        private void _Show()
        {
            gameObject.SetActive(true);
        }
    }
}