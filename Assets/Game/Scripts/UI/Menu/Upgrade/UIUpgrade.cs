using System;
using Game.Scripts.Data.Enums;
using Game.Scripts.Data.Project;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ValueType = Game.Scripts.Data.Enums.ValueType;

namespace Game.Scripts.UI.Menu.Upgrade
{
    public class UIUpgrade: MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image[] _stars;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _value;
        [SerializeField] private TMP_Text _buttonText;
        [SerializeField] private Button _upgradeButton;

        private UpgradesData _upgradesData;
        
        public Button Btn => _upgradeButton;
        
        [Inject]
        private void Construct(UpgradesData upgradesData)
        {
            _upgradesData = upgradesData;
        }
        
        public void Init(UpgradeType type)
        {
            var upgrade = _upgradesData.GetUpgrade(type);
            _icon.sprite = upgrade.Icon;
            _name.text = upgrade.Name;
            var lvl = _upgradesData.UpgradesLvl[type];
            for (int i = 0; i < upgrade.MaxLvl; i++)//
                  _stars[i].color = new Color(0,0,0,.4f);   
            for (int i = 0; i < lvl; i++)
                _stars[i].color = Color.white;
            switch (type)
            {
                case UpgradeType.GoldMultiplier:
                    _value.text = $"+{1+_upgradesData.GetValue(type):F1}x";
                    break;
                case UpgradeType.IncreaseGoldCount:
                    _value.text = $"+{_upgradesData.GetValue(type)*100}%";
                    break;
                case UpgradeType.SlowdownRockets:
                    _value.text = $"-{_upgradesData.GetValue(type)*100}%";
                    break;
                case UpgradeType.ReduceSpikesCount:
                    _value.text = $"-{_upgradesData.GetValue(type)*100}%";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            //_lvl.text = $"{_upgradesData.UpgradesLvl[type]} lv."; ToDo: Replace with stars
            switch (upgrade.ValueType)
            {
                case ValueType.Gold:
                    _buttonText.text = $"Upgrade <color=white>\n{_upgradesData.GetCost(type)}</color> g";
                    break;
                case ValueType.Data:
                    _buttonText.text = $"Upgrade <color=white>\n{_upgradesData.GetCost(type)}</color> d";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_upgradesData.IsMaxLvl(type))
            {
                _buttonText.text = "Max lvl";
                _upgradeButton.interactable = false;
            }
        }
    }
}