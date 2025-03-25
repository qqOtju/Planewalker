using Game.Scripts.Data.Config;
using Game.Scripts.Data.Enums;
using Game.Scripts.Data.GameScene;
using Game.Scripts.Data.Project;
using Game.Scripts.Other;
using LeanTween.Framework;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Logic.Mission.Objects
{
    public class Coin: MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _collectEffects;
        [SerializeField] private AudioClip _collectSound;
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private int _goldValue;
        
        private LocalGoldData _localGoldData;
        private GameStateData _gameStateData;
        private UpgradesData _upgradesData;
        private SoundManager _soundManager;
        private GoldData _goldData;
        private Vector3 _endScale;
        
        [Inject]
        private void Construct(GoldData goldData, LocalGoldData localGoldData, 
            GameStateData gameStateData, UpgradesData upgradesData, SoundManager soundManager)
        {
            _goldData = goldData;
            _localGoldData = localGoldData;
            _gameStateData = gameStateData;
            _upgradesData = upgradesData;
            _soundManager = soundManager;
        }
        
        private void Start()
        {
            _endScale = _sr.transform.localScale * 1.2f;
            Animation();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || _gameStateData.State != GameState.Game) return;
            foreach (var effect in _collectEffects)
            {
                effect.transform.SetParent(null);
                effect.Play();
            }
            _soundManager.PlaySound(_collectSound);
            var gold = _goldValue + (int)(_goldValue * _upgradesData.GetValue(UpgradeType.GoldMultiplier));
            _localGoldData.Count += gold;
            _goldData.Count += gold;
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            LeanTween.Framework.LeanTween.cancel(_sr.gameObject);
        }

        private void Animation()
        {
            LeanTween.Framework.LeanTween.scale(_sr.gameObject, _endScale, 0.5f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
        }
    }
}