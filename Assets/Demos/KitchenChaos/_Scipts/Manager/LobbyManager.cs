using System;
using System.Collections.Generic;
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
        public Lobby joinedLobby { get; private set; }
        public event Action OnStartCreate;
        public event Action OnCreateFailed;
        public event Action onJoinStart;
        public event Action onJoinFailed;
        public event Action onCodeJoinFailed;
        public event Action<List<Lobby>> onLobbyListUpdate;

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


        private bool IsLobbyHost()
        {
            return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
        }

        #region 查询房间

        public async void ListLobbies()
        {
            try
            {
                QueryLobbiesOptions options = new QueryLobbiesOptions
                {
                    //查询条件
                    Filters = new List<QueryFilter>
                    {
                        // 找到 所有的 公开的 可用空间 > 0 的房间
                        new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                    },
                };
                var lobbies = await LobbyService.Instance.QueryLobbiesAsync(options);
                onLobbyListUpdate?.Invoke(lobbies.Results);
            }
            catch (LobbyServiceException e)
            {
                Debug.LogWarning(e);
            }
        }

        #endregion

        #region 创建 退出

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

        public async void JoinWithID(string id)
        {
            onJoinStart?.Invoke();
            try
            {
                joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(id);
                GameManager.Instance.StartClient();
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                onCodeJoinFailed?.Invoke();
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

        #endregion

        #region 退出 删除 踢人

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

        #endregion

        #region 定时任务

        private void Start()
        {
            HeartBeatTask().Forget();
            QueryLobby().Forget();
        }

        private CancellationTokenSource _heartBeatCts;
        public int heartBeatInterval = 15;

        private async UniTask HeartBeatTask()
        {
            _heartBeatCts = new CancellationTokenSource();
            // 等待登录成功
            UniTask.WaitUntil(() => AuthenticationService.Instance.IsSignedIn, cancellationToken: _heartBeatCts.Token);
            //定时发送心跳
            while (!_heartBeatCts.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(heartBeatInterval), cancellationToken: _heartBeatCts.Token);
                if (IsLobbyHost())
                {
                    await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
                }
            }
        }

        private int queryInterval = 10;
        private CancellationTokenSource _queryCts;

        private async UniTask QueryLobby()
        {
            _queryCts = new CancellationTokenSource();
            // 等待登录成功
            UniTask.WaitUntil(() => AuthenticationService.Instance.IsSignedIn, cancellationToken: _queryCts.Token);

            //开始查询房间
            //仅当玩家没有加入房间时 才查询
            while (!_queryCts.IsCancellationRequested)
            {
                //如果玩家已经加入房间 则不再查询 但不退出查询任务
                if (joinedLobby == null)
                {
                    ListLobbies();
                }

                await UniTask.Delay(TimeSpan.FromSeconds(queryInterval), cancellationToken: _queryCts.Token);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _heartBeatCts?.Cancel();
            _queryCts?.Cancel();
            DeleteLobby();
        }

        #endregion
    }
}