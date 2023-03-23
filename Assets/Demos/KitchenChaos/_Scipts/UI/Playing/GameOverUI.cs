
using Nico.MVC;
using TMPro;
using UnityEngine;

namespace Kitchen.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI recipeDeliveredText;
        private GameObject _uiContainer;

        private void Awake()
        {
            _uiContainer = transform.Find("UIContainer").gameObject;
        }

        private void OnEnable()
        {
            GameManager.Instance.stateMachine.onStateChange += _OnGameStateChange;
            _uiContainer.SetActive(false);
        }
        private void OnDisable()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.stateMachine.onStateChange -= _OnGameStateChange;
        }

        private void _OnGameStateChange(GameState arg1, GameState arg2)
        {
            if (arg2 is GameOverState)
            {
                _uiContainer.SetActive(true);

                recipeDeliveredText.text = ModelManager.Get<CompletedOrderModel>().orderCount.ToString();
            }
        }
    }
}