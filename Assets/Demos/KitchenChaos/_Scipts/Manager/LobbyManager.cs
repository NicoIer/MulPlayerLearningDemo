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
            }
        }

        public async void QuickJoin()
        {
            try
            {
                joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
                GameManager.Instance.StartClient();
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        public Lobby GetCurrentLobby()
        {
            return joinedLobby;
        }

        public async void JoinWithCode(string code)
        {
            try
            {
                joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code);
                GameManager.Instance.StartClient();
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        private CancellationTokenSource _heartBeatCts;

        private void Start()
        {
            HeartBeatTask().Forget();
        }

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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _heartBeatCts?.Cancel();
        }
    }
}