using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kitchen.Scene
{
    public static class SceneLoader
    {
        public static async void Load(string targetSceneName, string transitionSceneName)
        {
            Debug.Log("Load1");
            await SceneManager.LoadSceneAsync(transitionSceneName);
            Debug.Log("Load2");
            await Task.Delay(TimeSpan.FromSeconds(0.2f));
            Debug.Log("Load3");
            await SceneManager.LoadSceneAsync(targetSceneName);
            Debug.Log("Load4");
        }
    }
}