using System;
using Kitchen.Manager;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.Visual
{
    public class CharacterSelect : MonoBehaviour
    {
        [SerializeField] private int playerIdx;
        [SerializeField] private GameObject readyObj;
        [SerializeField] private PlayerVisual playerVisual;
        [SerializeField] private Button kickButton;
        [SerializeField] private TextMeshPro nameText;

        private void Awake()
        {
            if (playerVisual == null)
                playerVisual = GetComponent<PlayerVisual>();
        }

        private void Start()
        {
            GameManager.Instance.playerConfigs.OnListChanged += _OnPlayerConfigListChange;
            SelectManager.Instance.onReadyChange += _OnReadyChange;
            kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
            Debug.Log("注册点击事件");
            kickButton.onClick.AddListener(() =>
            {
                Debug.Log("kick");
                var config = GameManager.Instance.playerConfigs[playerIdx];
                GameManager.Instance.KickPlayer(config.clientId);
            });
            UpdateVisual();
        }


        private void _OnReadyChange()
        {
            UpdateVisual();
        }

        private void _OnPlayerConfigListChange(NetworkListEvent<PlayerConfig> changeevent)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (playerIdx < GameManager.Instance.playerConfigs.Count)
            {
                //获取玩家配置
                PlayerConfig config = GameManager.Instance.playerConfigs[playerIdx];
                gameObject.SetActive(true);
                //判断玩家是否准备
                readyObj.SetActive(SelectManager.Instance.readyClientSet.Contains(config.clientId));

                //获取玩家颜色
                var color = GameManager.Instance.GetColor(config.colorId);
                //设置玩家颜色
                playerVisual.SetColor(color);
                nameText.text = config.playerName.ToString();
            }
            else
            {
                gameObject.SetActive(false);
                readyObj.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            GameManager.Instance.playerConfigs.OnListChanged -= _OnPlayerConfigListChange;
        }
    }
}