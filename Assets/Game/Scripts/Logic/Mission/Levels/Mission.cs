using System;
using System.Collections;
using Game.Scripts.Data.Config;
using Game.Scripts.Data.Enums;
using Game.Scripts.Data.GameScene;
using Game.Scripts.Data.Project;
using Game.Scripts.Logic.Mission.Objects;
using Game.Scripts.UI.Game;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Scripts.Logic.Mission.Levels
{
    public class Mission: MonoBehaviour
    {
        [SerializeField] private Cloud _cloud;
        [SerializeField] private float _time;
        [Header("Spawn Points")]
        [SerializeField] private Transform _planeSpawnPoint;
        [SerializeField] private Transform[] _spikeSpawnPoints;
        [SerializeField] private Transform[] _coinSpawnPoints;
        [SerializeField] private Transform[] _cannonSpawnPoints;
        [Header("Prefabs")]
        [SerializeField] private Coin _coinPrefab;
        [SerializeField] private Spike _spikePrefab;
        [SerializeField] private Cannon _cannonPrefab;
        
        private DiContainer _container;
        private UpgradesData _upgradesData;
        private Plane _plane;
        
        protected GameStateData GameStateData;
        protected UIResults UIResults; 
        protected DataData DataData;
        protected LevelData LevelData;

        public Action<string> OnUIUpdate; 
        public event Action<float> OnTimerUpdate;
        
        [Inject]
        private void Construct(DiContainer container, UIResults uiResults, 
            Plane plane, GameStateData gameStateData, DataData dataData, 
            LevelData levelData, UpgradesData upgradesData)
        {
            _container = container;
            UIResults = uiResults;
            _plane = plane;
            GameStateData = gameStateData;
            DataData = dataData;
            LevelData = levelData;
            _upgradesData = upgradesData;
        }
        
        protected virtual void Start()
        {
            _cloud.Init(UIResults, GameStateData);
            _plane.transform.position = _planeSpawnPoint.position;
            SpawnGold();
            SpawnSpkies();
            SpawnObjects(_cannonPrefab, _cannonSpawnPoints);
            StartCoroutine(Timer());
        }

        private void SpawnObjects(MonoBehaviour prefab, Transform[] spawnPoints)
        {
            foreach (var spawnPoint in spawnPoints)
            {
                var obj = _container.InstantiatePrefab(prefab);
                obj.transform.position = spawnPoint.position;
            }
        }

        private void SpawnGold()
        {
            var count = _coinSpawnPoints.Length / 2 +
                        (int)(_cannonSpawnPoints.Length * _upgradesData.GetValue(UpgradeType.IncreaseGoldCount));
            var usedSpawnPoints = new bool[_coinSpawnPoints.Length];
            for (var i = 0; i < count; i++)
            {
                var obj = _container.InstantiatePrefab(_coinPrefab);
                var spawnPointIndex = Random.Range(0, _coinSpawnPoints.Length);
                while (usedSpawnPoints[spawnPointIndex])
                    spawnPointIndex = Random.Range(0, _coinSpawnPoints.Length);
                usedSpawnPoints[spawnPointIndex] = true;
                obj.transform.position = _coinSpawnPoints[spawnPointIndex].position;
            }
        }

        private void SpawnSpkies()
        {
            var count = _spikeSpawnPoints.Length - (int)(_spikeSpawnPoints.Length * _upgradesData.GetValue(UpgradeType.ReduceSpikesCount));
            var usedSpawnPoints = new bool[_spikeSpawnPoints.Length];
            for (var i = 0; i < count; i++)
            {
                var obj = _container.InstantiatePrefab(_spikePrefab);
                var spawnPointIndex = Random.Range(0, _spikeSpawnPoints.Length);
                while (usedSpawnPoints[spawnPointIndex])
                    spawnPointIndex = Random.Range(0, _spikeSpawnPoints.Length);
                usedSpawnPoints[spawnPointIndex] = true;
                obj.transform.position = _spikeSpawnPoints[spawnPointIndex].position;
            }
        }

        private IEnumerator Timer()
        {
            var time = _time;
            while (time > 0)
            {
                time -= Time.deltaTime;
                OnTimerUpdate?.Invoke(time);
                yield return null;
            }
            if (GameStateData.State != GameState.Game) yield break;
            GameStateData.State = GameState.Death;
            UIResults.Show("Out of time");
        }
    }
}