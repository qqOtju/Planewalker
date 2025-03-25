using System.Collections.Generic;
using Game.Scripts.Data.Enums;
using Game.Scripts.Data.GameScene;
using Game.Scripts.Logic.Mission.Objects;
using UnityEngine;

namespace Game.Scripts.Logic.Mission.Levels
{
    public class ProtectionLevel: Mission
    {
        [SerializeField] private FlyPoint[] _flyPoints;
        
        private readonly List<FlyPoint> _collectedPoints = new ();
        private int _collectedCount;
        private bool _isComplete;
        

        protected override void Start()
        {
            base.Start();
            foreach (var point in _flyPoints)
                point.OnCollected += OnCollected;
            OnUIUpdate?.Invoke($"{_collectedCount}/{_flyPoints.Length}\nPoints");
        }

        private void OnCollected(FlyPoint obj)
        {
            if(_collectedPoints.Contains(obj) || _isComplete) return;
            _collectedPoints.Add(obj);
            _collectedCount++;
            OnUIUpdate?.Invoke($"{_collectedCount}/{_flyPoints.Length}\nPoints");
            if (_collectedCount < _flyPoints.Length) return;
            _isComplete = true;
            GameStateData.State = GameState.Complete;
            DataData.Data += LevelData.MissionData.Mission.DataPrize;
            UIResults.Show("Complete");
        }
    }
}