using Game.Scripts.Data.Config;
using Game.Scripts.Data.Enums;
using Game.Scripts.Data.GameScene;
using Game.Scripts.Data.Project;
using Game.Scripts.Other;
using Game.Scripts.UI.Game;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Logic.Mission.Objects
{
    public class Rocket: MonoBehaviour
    {
        [SerializeField] private ParticleSystem _explosion;
        [SerializeField] private float _baseSpeed;

        private GameStateData _gameStateData;
        private UpgradesData _upgradesData;
        private UIResults _uiResults;
        private float _speed;
        
        [Inject]
        private void Construct(UIResults uiResults, GameStateData gameStateData, UpgradesData upgradesData)
        {
            _uiResults = uiResults;
            _gameStateData = gameStateData;
            _upgradesData = upgradesData;
        }

        private void Start()
        {
            _speed = _baseSpeed - (_baseSpeed * _upgradesData.GetValue(UpgradeType.SlowdownRockets));
            Destroy(gameObject, 20f);
        }

        private void FixedUpdate()
        {
            transform.Translate(Vector3.up * (_speed * Time.fixedDeltaTime));
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || _gameStateData.State != GameState.Game) return;
            _explosion.transform.SetParent(null);
            _explosion.Play();
            Destroy(gameObject);
            _gameStateData.State = GameState.Death;
            _uiResults.Show("BOOM");
        }
    }
}