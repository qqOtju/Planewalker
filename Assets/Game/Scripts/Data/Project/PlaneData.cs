using Game.Scripts.Data.Config;
using Game.Scripts.Other;
using UnityEngine;

namespace Game.Scripts.Data.Project
{
    public class PlaneData
    {
        private const string Key = "Plane";
        private const string LvlKey = "Lvl";
        
        private State _state;
        private int _lvl;

        public SOPlane Plane { get; }
        
        public State State
        {
            get => _state;
            set
            {
                _state = value;
                PlayerPrefs.SetInt($"{Key}{Plane.Id}", (int) State);
            }
        }
        
        public int Lvl
        {
            get => _lvl;
            set
            {
                _lvl = value;
                PlayerPrefs.SetInt($"{Key}{LvlKey}{Plane.Id}", Lvl);
            }
        }
        
        public float MaxSpeed => Plane.MaxSpeed + Lvl * 0.1f;
        public float Acceleration => Plane.Acceleration + Lvl * 0.1f;
        public float Controllability => Plane.Controllability + Lvl * 0.1f;
        public int UpgradePrice => Plane.UpgradePrice + Lvl * 100;

        public PlaneData(SOPlane plane)
        {
            Plane = plane;
            _state = (State) PlayerPrefs.GetInt($"{Key}{Plane.Id}", 0);
            if(plane.Id == 1 || plane.Id == 4 || plane.Id == 7)
                _state = State.Unlocked;
            _lvl = PlayerPrefs.GetInt($"{Key}{LvlKey}{Plane.Id}", 0);
        }
    }
}