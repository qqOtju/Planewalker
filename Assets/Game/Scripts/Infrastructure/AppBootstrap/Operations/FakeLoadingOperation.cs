using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Infrastructure.AppBootstrap.Operations
{
    public class FakeLoadingOperation: ILoadingOperation
    {
        private readonly float _timeToLoad;
        private readonly int _sceneIndexToLoad;
        private readonly int _sceneIndexToUnload;
        
        public FakeLoadingOperation(float timeToLoad, int sceneIndexToLoad, int sceneIndexToUnload)
        { 
            _timeToLoad = timeToLoad;
            _sceneIndexToLoad = sceneIndexToLoad;
            _sceneIndexToUnload = sceneIndexToUnload;
        }
        
        public Task UseOperation(Action<float> onProgress)
        {
            SceneManager.LoadSceneAsync(_sceneIndexToLoad, LoadSceneMode.Additive);
            var source = new TaskCompletionSource<bool>();
            async Task LoadScene()
            {
                var time = 0f;
                while (time < _timeToLoad)
                {
                    onProgress?.Invoke(time / _timeToLoad);
                    time += Time.deltaTime;
                    await Task.Yield();
                }
                source.SetResult(true);
            }
            _ = LoadScene();
            SceneManager.UnloadSceneAsync(_sceneIndexToUnload);
            return source.Task;
        }
    }
}