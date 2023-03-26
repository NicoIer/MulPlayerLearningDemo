using System;
using Nico.Exception;
using Unity.Netcode;
using UnityEngine;

namespace Kitchen.Manager
{
    public class MainMenuManager : MonoBehaviour
    {
        public void Awake()
        {
            if (NetworkManager.Singleton != null)
            {
                Destroy(NetworkManager.Singleton.gameObject);
            }

            try
            {
                Destroy(GameManager.Instance.gameObject);
                Destroy(LobbyManager.Instance.gameObject);
            }
            catch (SingletonException e)
            {
            }
            
        }
    }
}