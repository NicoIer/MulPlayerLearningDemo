using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Kitchen.Scene
{
    //TODO 后续改到GlobalManager里去
    public static class SceneLoader
    {
        public static async void Load(string targetSceneName, string transitionSceneName)
        {
            await SceneManager.LoadSceneAsync(transitionSceneName);
            await Task.Delay(TimeSpan.FromSeconds(0.2f));
            await SceneManager.LoadSceneAsync(targetSceneName);
        }
    }
}