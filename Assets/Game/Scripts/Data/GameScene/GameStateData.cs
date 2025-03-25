using System;
using Game.Scripts.Data.Enums;

namespace Game.Scripts.Data.GameScene
{
    public class GameStateData
    {
        public GameState State { get; set; }

        public GameStateData() => State = GameState.Game;
    }
}