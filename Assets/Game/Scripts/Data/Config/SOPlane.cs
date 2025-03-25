using Game.Scripts.Logic.Mission;
using UnityEngine;

namespace Game.Scripts.Data.Config
{
    [CreateAssetMenu(fileName = "Plane", menuName = "MyAssets/Plane")]
    public class SOPlane: ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;
        [SerializeField] private MissionType _missionType;
        [SerializeField] private int _id;
        [SerializeField] private int _price;
        [SerializeField] private int _upgradePrice;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _controllability;
        
        public Sprite Icon => _icon;
        public int Price => _price;
        public string Name => _name;
        public int Id => _id;
        public float MaxSpeed => _maxSpeed;
        public float Acceleration => _acceleration;
        public float Controllability => _controllability;
        public MissionType MissionType => _missionType;
        public int UpgradePrice => _upgradePrice;
    }
}