using Game.Scripts.Data.Enums;
using Game.Scripts.Logic.Mission;
using Game.Scripts.Logic.Mission.Levels;
using UnityEngine;

namespace Game.Scripts.Data.Config
{
    [CreateAssetMenu(fileName = "Mission", menuName = "MyAssets/Mission")]
    public class SOMission: ScriptableObject
    {
        [SerializeField] private Mission _mission;
        [SerializeField] private Sprite _icon;
        [SerializeField] private MissionType _missionType;
        [SerializeField] private Difficulty _difficulty;
        [SerializeField] private int _dataPrice;
        [SerializeField] private int _dataPrize;
        [SerializeField] private int _id;
        
        public Mission Mission => _mission;
        public Sprite Icon => _icon;
        public MissionType MissionType => _missionType;
        public Difficulty Difficulty => _difficulty;
        public int DataPrice => _dataPrice;
        public int DataPrize => _dataPrize;
        public int Id => _id;
    }
}