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
                var config = GameManager.Instance.playerConfigs[playerIdx];
                gameObject.SetActive(true);
                if (SelectManager.Instance.readyClientSet.Contains(config.clientId))
                {
                    readyText.gameObject.SetActive(true);
                }
                else
                {
                    readyText.gameObject.SetActive(false);
                }

                var color = GameManager.Instance.GetColor(playerIdx);

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