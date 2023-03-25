using System;
using Kitchen.Config;
using Kitchen.Scene;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] public Button createGameButton;
        [SerializeField] public Button joinGameButton;

        private void Awake()
        {
            createGameButton.onClick.AddListener(() =>
            {
                GameManager.Instance.StartHost();
                SceneLoader.LoadNet(SceneName.CharacterSelectScene,SceneName.LoadingScene);
            });

            joinGameButton.onClick.AddListener(() =>
            {
                GameManager.Instance.StartClient();
            });
        }
    }
}