using System.Collections.Generic;
using Game.Scripts.Data.Enums;
using Game.Scripts.Data.GameScene;
using Game.Scripts.Logic.Mission.Objects;
using UnityEngine;

namespace Game.Scripts.Logic.Mission.Levels
{
    public class ExplorationLevel: Mission
    {
        [SerializeField] private FlyPoint[] _areas;

        private readonly List<FlyPoint> _collectedAreas = new ();
        private int _collectedCount;
        private bool _isComplete;

        protected override void Start()
        {
            base.Start();
            foreach (var zone in _areas)
                zone.OnCollected += OnCollected;
            OnUIUpdate?.Invoke($"{_collectedCount}/{_areas.Length}\nAreas");
        }

        private void OnCollected(FlyPoint obj)
        {
            if(_collectedAreas.Contains(obj) || _isComplete) return;
            _collectedAreas.Add(obj);
            _collectedCount++;
            OnUIUpdate?.Invoke($"{_collectedCount}/{_areas.Length}\nAreas");
            if (_collectedCount < _areas.Length) return;
            _isComplete = true;
            GameStateData.State = GameState.Complete;
            DataData.Data += LevelData.MissionData.Mission.DataPrize;
            UIResults.Show("Complete");
        }
    }
}