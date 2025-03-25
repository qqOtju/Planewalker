using System;
using Game.Scripts.Data.Enums;
using Game.Scripts.Data.GameScene;
using Game.Scripts.UI.Game;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Logic.Mission.Objects
{
    public class Spike: MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private Transform _view;

        private GameStateData _gameStateData;
        private UIResults _uiResults;
        private Vector3 _axis;
        private float _angle;
        
        [Inject]
        private void Construct(UIResults uiResults, GameStateData gameStateData)
        {
            _uiResults = uiResults;
            _gameStateData = gameStateData;
        }

        private void Start()
        {
            _angle = UnityEngine.Random.Range(0.5f, 1.5f);
            var random = UnityEngine.Random.Range(-1, 1);
            switch (random)
            {
                case -1:
                    _axis = Vector3.forward;
                    break;
                case 0:
                    _axis = Vector3.zero;
                    break;
                case 1:
                    _axis = Vector3.up;
                    break;
            }
        }

        private void Update()
        {
            _view.Rotate(_axis, _angle);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")|| _gameStateData.State != GameState.Game) return;
            _particleSystem.transform.SetParent(null);
            _particleSystem.Play();
            Destroy(gameObject);
            _gameStateData.State = GameState.Death;
            _uiResults.Show("BOOM");
        }
    }
}