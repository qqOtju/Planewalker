using Game.Scripts.Data.Config;
using Game.Scripts.Other;
using UnityEngine;

namespace Game.Scripts.Data.Project
{
    public class MissionData
    {
        private State _state;
        
        public SOMission Mission { get; }

        public State State
        {
            get => _state;
            set
            {
                _state = value;
                PlayerPrefs.SetInt($"MissionData{Mission.Id}", (int) State);
            }
        }
        
        public MissionData(SOMission mission)
        {
            Mission = mission;
            _state = (State) PlayerPrefs.GetInt($"MissionData{mission.Id}", 0);
            if(mission.Id == 1 || mission.Id == 4 || mission.Id == 7)
                _state = State.Unlocked;
        }
    }
}