using Cysharp.Threading.Tasks;
using Kitchen.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Kitchen.UI.Menu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;
        
        private void Start()
        {
            playButton.onClick.AddListener(_OnPlayButtonClick);
            quitButton.onClick.AddListener(_OnQuitButtonClick);
        }
        
        private void _OnPlayButtonClick()
        {
            GameManager.Instance.ExitGame();
            SceneLoader.Load("GameScene","LoadingScene");
        }
        
        private void _OnQuitButtonClick()
        {
            Application.Quit();
        }
        
    }
}