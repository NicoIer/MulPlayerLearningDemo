using Kitchen.Config;
using Kitchen.Manager;
using Kitchen.Scene;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class CharacterSelectUI : MonoBehaviour
    {
        [SerializeField] private Button readyButton;
        [SerializeField] private Button mainMenuButton;

        private void Awake()
        {
            mainMenuButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.Shutdown();
                SceneLoader.Load(SceneName.MainMenuScene);
            });
            readyButton.onClick.AddListener(() => { SelectManager.Instance.SetPlayerReadyServerRpc(); });
        }
    }
}