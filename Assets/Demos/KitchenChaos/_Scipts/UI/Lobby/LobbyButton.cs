using Kitchen.Manager;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class LobbyButton : MonoBehaviour
    {
        private Lobby lobby;
        [field: SerializeField] public Button lobbyBtn { get; private set; }
        [SerializeField] private TextMeshProUGUI lobbyNameText;

        public void SetLobby(Lobby lobby)
        {
            lobbyBtn.onClick.RemoveAllListeners();
            lobbyBtn.onClick.AddListener(() => { LobbyManager.Instance.JoinWithID(lobby.Id); });
            this.lobby = lobby;
            lobbyNameText.text = lobby.Name;
        }
    }
}