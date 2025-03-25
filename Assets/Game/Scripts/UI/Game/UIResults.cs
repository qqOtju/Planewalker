using System;
using Game.Scripts.Ads;
using Game.Scripts.Data.Enums;
using Game.Scripts.Data.GameScene;
using Game.Scripts.Data.Project;
using Game.Scripts.Other;
using Game.Scripts.UI.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.UI.Game
{
    [RequireComponent(typeof(CanvasGroup), typeof(Canvas))]
    public class UIResults : MonoBehaviour
    {
        [SerializeField] private GameObject _winUI;
        [SerializeField] private GameObject _loseUI;
        [SerializeField] private TMP_Text _winTitleText;
        [SerializeField] private TMP_Text _loseTitleText;
        [SerializeField] private TMP_Text _gainedDataText;
        [SerializeField] private TMP_Text _gainedGold;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Transform _content;
        [SerializeField] private AudioClip _winSound;
        [SerializeField] private AudioClip _loseSound;
        
        private UIAnimation _uiAnimation;
        private GameStateData _gameStateData;
        private LevelData _levelData;
        private LocalGoldData _localGoldData;
        private SoundManager _soundManager;
        private AdMobAds _ads;
        private bool _isShown;
        
        [Inject]
        private void Construct(LocalGoldData localGoldData, 
            GameStateData gameStateData, LevelData levelData, SoundManager soundManager, AdMobAds ads)
        {
            _localGoldData = localGoldData;
            _gameStateData = gameStateData;
            _levelData = levelData;
            _soundManager = soundManager;
            _ads = ads;
        }
        
        private void Awake()
        {
            var canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            _uiAnimation = new UIAnimation(_content, canvas, 3);
            _menuButton.onClick.AddListener(OnMenuButtonClick);
            _restartButton.onClick.AddListener(OnRestartButtonClick);
        }

        private void Start()
        {
            _ads.RequestInterstitial();
        }

        private void OnDestroy()
        {
            _menuButton.onClick.RemoveAllListeners();
            _restartButton.onClick.RemoveAllListeners();
            StopAllCoroutines();
        }
        
        public void Show(string title)
        {
            if (_isShown) return;
            _isShown = true;
            _gainedGold.text = $"+{_localGoldData.Count}";
            if (_gameStateData.State == GameState.Complete)
            {
                _soundManager.PlaySound(_winSound);
                _winTitleText.text = title;
                _gainedDataText.text = $"+{_levelData.MissionData.Mission.DataPrize}";
                _loseUI.gameObject.SetActive(false);
                _winUI.gameObject.SetActive(true);
            }
            else if(_gameStateData.State == GameState.Death)
            {
                _soundManager.PlaySound(_loseSound);
                _loseTitleText.text = title;
                _gainedDataText.text = "+0";
                _loseUI.gameObject.SetActive(true);
                _winUI.gameObject.SetActive(false);
            }
            StartCoroutine(_uiAnimation.Show());
            _ads.ShowInterstitial();
        }

        private void OnMenuButtonClick()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        private void OnRestartButtonClick()
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
    }
}