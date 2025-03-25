using Game.Scripts.Data.Enums;
using Game.Scripts.Data.GameScene;
using Game.Scripts.UI.Game;
using UnityEngine;

namespace Game.Scripts.Logic.Mission.Objects
{
    public class Cloud: MonoBehaviour
    {
        private GameStateData _gameStateData;
        private UIResults _uiResults;
        
        public void Init(UIResults uiResults, GameStateData gameStateData)
        {
            _uiResults = uiResults;
            _gameStateData = gameStateData;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || _gameStateData.State != GameState.Game) return;
            _gameStateData.State = GameState.Death;
            _uiResults.Show("Lost in Clouds");
        }
    }
}