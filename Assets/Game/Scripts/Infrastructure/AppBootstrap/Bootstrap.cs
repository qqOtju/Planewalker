using System;
using System.Collections.Generic;
using Game.Scripts.Ads;
using Game.Scripts.Infrastructure.AppBootstrap.Operations;
using Game.Scripts.UI.FirstScene;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game.Scripts.Infrastructure.AppBootstrap
{
    public class Bootstrap: MonoBehaviour
    {
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private float _timeToLoad = 7.7f;

        private AdMobAds _ads;
        
        [Inject]
        private void Construct(AdMobAds ads)
        {
            _ads = ads;
        }
        
        private async void Awake()
        {
            var loadingOperations = new Queue<ILoadingOperation>();
            loadingOperations.Enqueue(new FakeLoadingOperation(_timeToLoad, 
                1, SceneManager.GetActiveScene().buildIndex));
            var loadingScreen = Instantiate(_loadingScreen);
            await loadingScreen.Load(loadingOperations);
            Application.targetFrameRate = 60;
            _ads.RequestInterstitial();
            Destroy(gameObject);
        }
    }
}