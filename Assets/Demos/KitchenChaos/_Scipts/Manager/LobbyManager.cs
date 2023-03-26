using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kitchen.Config;
using Kitchen.Scene;
using Nico.Design;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kitchen.Manager
{
    public class LobbyManager : GlobalSingleton<LobbyManager>, IInitializable
    {
        public int heartBeatInterval = 15;
        private Lobby joinedLobby;
        public event Action OnStartCreate;
        public event Action OnCreateFailed;
        public event Action onJoinStart;
        public event Action onJoinFailed;
        public event Action onCodeJoinFailed;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        public async void Initialize()
        {
            if (UnityServices.State == ServicesInitializationState.Initialized)
            {
                return;
            }

            var options = new InitializationOptions();
            options.SetProfile(Random.Range(0, 1000).ToString());
            await UnityServices.InitializeAsync(options);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        public async void Create(string lobbyName, bool isPrivate)
        {
            OnStartCreate?.Invoke();
            try
            {
                var options = new CreateLobbyOptions();
                options.IsPrivate = isPrivate;
                joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName,
                    GameManager.Instance.setting.maxPlayerCount, options);

                GameManager.Instance.StartHost();
                SceneLoader.LoadNet(SceneName.CharacterSelectScene, SceneName.LoadingScene);
            }
            catch (LobbyServiceException e)
            {
                Debug.LogWarning(e);
                OnCreateFailed?.Invoke();
            }
        }

        public async void QuickJoin()
        {
            onJoinStart?.Invoke();
            try
            {
                joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
                GameManager.Instance.StartClient();
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                onJoinFailed?.Invoke();
            }
        }


        public async void JoinWithCode(string code)
        {
            onJoinStart?.Invoke();
            try
            {
                joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code);
                GameManager.Instance.StartClient();
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                onCodeJoinFailed?.Invoke();
            }
        }

        private void Start()
        {
            HeartBeatTask().Forget();
        }

        public Lobby GetCurrentLobby()
        {
            return joinedLobby;
        }

        private CancellationTokenSource _heartBeatCts;


        private async UniTask HeartBeatTask()
        {
            _heartBeatCts = new CancellationTokenSource();
            await UniTask.Delay(TimeSpan.FromSeconds(heartBeatInterval), cancellationToken: _heartBeatCts.Token);
            if (IsLobbyHost())
            {
                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }

        private bool IsLobbyHost()
        {
            return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
        }

        public async void DeleteLobby()
        {
            if (joinedLobby != null)
            {
                try
                {
                    await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
                    joinedLobby = null;
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e);
                }
            }
        }

        public async void LeaveLobby()
        {
            try
            {
                if (joinedLobby != null)
                {
                    await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id,
                        AuthenticationService.Instance.PlayerId);
                    joinedLobby = null;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        public async void KickPlayer(string playerId)
        {
            try
            {
                if (IsLobbyHost())
                {
                    await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _heartBeatCts?.Cancel();
            DeleteLobby();
        }
    }
}