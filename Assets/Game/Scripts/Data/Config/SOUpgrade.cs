using Game.Scripts.Data.Enums;
using UnityEngine;

namespace Game.Scripts.Data.Config
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "MyAssets/Upgrade")]
    public class SOUpgrade: ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField] private UpgradeType _type;
        [SerializeField] private ValueType _valueType;
        [SerializeField] private float _baseValue;
        [SerializeField] private int _upgradeCost;
        [SerializeField] private int _maxLvl;
        
        public Sprite Icon => _icon;
        public string Name => _name;
        public UpgradeType Type => _type;
        public float BaseValue => _baseValue;
        public int UpgradeCost => _upgradeCost;
        public ValueType ValueType => _valueType;
        public int MaxLvl => _maxLvl;
    }
}