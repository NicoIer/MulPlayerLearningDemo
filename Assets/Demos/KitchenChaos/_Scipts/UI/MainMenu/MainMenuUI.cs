using System;
using Cysharp.Threading.Tasks;
using Kitchen.Config;
using Kitchen.Scene;
using Unity.Netcode;
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
            SceneLoader.Load(SceneName.LobbyScene, "LoadingScene");
        }

        private void _OnQuitButtonClick()
        {
            Application.Quit();
        }
    }
}