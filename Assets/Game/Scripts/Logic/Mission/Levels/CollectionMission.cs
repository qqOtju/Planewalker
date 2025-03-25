using System.Collections.Generic;
using Game.Scripts.Data.Enums;
using Game.Scripts.Data.GameScene;
using Game.Scripts.Logic.Mission.Objects;
using UnityEngine;

namespace Game.Scripts.Logic.Mission.Levels
{
    public class CollectionMission: Mission
    {
        [SerializeField] private CollectionZone[] _zones;

        private readonly List<CollectionZone> _collectedZones = new ();
        private int _collectedCount;
        private bool _isComplete;

        protected override void Start()
        {
            base.Start();
            foreach (var zone in _zones)
                zone.OnCollected += OnCollected;
            OnUIUpdate?.Invoke($"{_collectedCount}/{_zones.Length}\nZones");
        }

        private void OnCollected(CollectionZone obj)
        {
            if(_collectedZones.Contains(obj) || _isComplete) return;
            _collectedZones.Add(obj);
            _collectedCount++;
            OnUIUpdate?.Invoke($"{_collectedCount}/{_zones.Length}\nZones");
            if (_collectedCount < _zones.Length) return;
            _isComplete = true;
            GameStateData.State = GameState.Complete;
            DataData.Data += LevelData.MissionData.Mission.DataPrize;
            UIResults.Show("Complete");
        }
    }
}