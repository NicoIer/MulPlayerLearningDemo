using System;
using Kitchen.Manager;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Kitchen.Visual
{
    public class CharacterSelect : MonoBehaviour
    {
        [SerializeField] private int playerIdx;
        [SerializeField] private TextMeshPro readyText;
        [SerializeField] private PlayerVisual playerVisual;

        private void Awake()
        {
            playerVisual = GetComponent<PlayerVisual>();
        }

        private void Start()
        {
            GameManager.Instance.playerConfigs.OnListChanged += _OnPlayerConfigListChange;
            SelectManager.Instance.onReadyChange += _OnReadyChange;
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
                var config = GameManager.Instance.playerConfigs[playerIdx];
                gameObject.SetActive(true);
                //判断玩家是否准备
                readyText.gameObject.SetActive(SelectManager.Instance.readyClientSet.Contains(config.clientId));

                //获取玩家颜色
                var color = GameManager.Instance.GetColor(config.colorId);
                //设置玩家颜色
                playerVisual.SetColor(color);
            }
            else
            {
                gameObject.SetActive(false);
                readyText.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            GameManager.Instance.playerConfigs.OnListChanged -= _OnPlayerConfigListChange;
        }
    }
}