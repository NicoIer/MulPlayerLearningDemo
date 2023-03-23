
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class DeliveryResultUI : MonoBehaviour
    {
        private readonly int _popUpAnimParam = Animator.StringToHash("pop_up");
        [SerializeField] private Animator animator;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Color successColor;
        [SerializeField] private Color failColor;
        [SerializeField] private Sprite successSprite;
        [SerializeField] private Sprite failSprite;

        private void Start()
        {
            DeliveryManager.Instance.OnOrderSuccess += OnOrderSuccess;
            DeliveryManager.Instance.OnOrderFailed += OnOrderFail;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            var deliveryManager = DeliveryManager.GetInstanceUnSafe();
            if (deliveryManager != null)
            {
                deliveryManager.OnOrderSuccess -= OnOrderSuccess;
                deliveryManager.OnOrderFailed -= OnOrderFail;
                return;
            }
        }

        public void OnAnimFinish()
        {
            gameObject.SetActive(false);
        }
        private void OnOrderFail(object sender, Vector3 e)
        {
            gameObject.SetActive(true);
            backgroundImage.color = failColor;
            iconImage.sprite = failSprite;
            messageText.text = "Order Failed";
            animator.SetTrigger(_popUpAnimParam);
        }

        private void OnOrderSuccess(object sender, Vector3 e)
        {
            gameObject.SetActive(true);
            backgroundImage.color = successColor;
            iconImage.sprite = successSprite;
            messageText.text = "Order Success";
            animator.SetTrigger(_popUpAnimParam); 
        }
    }
}