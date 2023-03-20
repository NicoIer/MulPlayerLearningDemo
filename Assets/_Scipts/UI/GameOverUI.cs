using Kitchen.Model;
using Nico.MVC;
using TMPro;
using UnityEngine;

namespace Kitchen.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI recipeDeliveredText;

        private void Start()
        {
            GameManager.Instance.stateMachine.onStateChange += _OnGameStateChange;
            gameObject.SetActive(false);
        }

        private void _OnGameStateChange(GameState arg1, GameState arg2)
        {
            if (arg2 is GameOverState)
            {
                gameObject.SetActive(true);

                recipeDeliveredText.text = ModelManager.Get<CompletedOrderModel>().orderCount.ToString();
            }
        }

        private void OnDestroy()
        {
            GameManager.Instance.stateMachine.onStateChange -= _OnGameStateChange;
        }
    }
}