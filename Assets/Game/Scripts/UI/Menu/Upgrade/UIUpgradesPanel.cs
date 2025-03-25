using System;
using System.Collections;
using Game.Scripts.Data.Enums;
using Game.Scripts.Data.Project;
using LeanTween.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using ValueType = Game.Scripts.Data.Enums.ValueType;

namespace Game.Scripts.UI.Menu.Upgrade
{
    public class UIUpgradesPanel: UIPanelTemplate
    {
        [SerializeField] private UIUpgrade[] _upgrades;
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private TMP_Text _dataText;
        [SerializeField] private CanvasGroup _alert;
        [SerializeField] private TMP_Text _alertText;
        
        private UpgradesData _upgradesData;
        private GoldData _goldData;
        private DataData _dataData;
        
        [Inject]
        private void Construct(UpgradesData upgradesData, GoldData goldData, DataData dataData)
        {
            _upgradesData = upgradesData;
            _goldData = goldData;
            _dataData = dataData;
        }
        
        protected override void Awake()
        {
            base.Awake();
            _goldData.OnCountChanged += OnCountChanged;
            _dataData.OnDataCountChanged += OnDataCountChanged;
            _goldText.text = $"{_goldData.Count.ToString()} G";
            _dataText.text = $"{_dataData.Data.ToString()} D";
            for(int i = 0; i < _upgrades.Length; i++)
            {
                var type = (UpgradeType)i;
                _upgrades[i].Init(type);
                _upgrades[i].Btn.onClick.AddListener(() => Upgrade(type));
            }
        }

        private void Upgrade(UpgradeType upgradeType)
        {
            switch (_upgradesData.GetUpgrade(upgradeType).ValueType)
            {
                case ValueType.Gold:
                    UpgradeGold(upgradeType);
                    break;
                case ValueType.Data:
                    UpgradeData(upgradeType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void UpgradeData(UpgradeType upgradeType)
        {
            var upgrade = _upgradesData.GetUpgrade(upgradeType);
            if(_upgradesData.GetCost(upgradeType) > _dataData.Data)
            {
                StopAllCoroutines();
                StartCoroutine(Alert(upgrade.ValueType));
                return;
            }
            _dataData.Data -= _upgradesData.GetCost(upgradeType);
            _upgradesData.IncreaseUpgradeLvl(upgradeType);
            for(int i = 0; i < _upgrades.Length; i++)
            {
                _upgrades[i].Init((UpgradeType)i);
            }
        }
        
        private void UpgradeGold(UpgradeType upgradeType)
        {
            var upgrade = _upgradesData.GetUpgrade(upgradeType);
            if(_upgradesData.GetCost(upgradeType) > _goldData.Count)
            {
                StopAllCoroutines();
                StartCoroutine(Alert(upgrade.ValueType));
                return;
            }
            _goldData.Count -= _upgradesData.GetCost(upgradeType);
            _upgradesData.IncreaseUpgradeLvl(upgradeType);
            for(int i = 0; i < _upgrades.Length; i++)
            {
                _upgrades[i].Init((UpgradeType)i);
            }
        }

        private void OnDataCountChanged(int obj)
        {
            _dataText.text = $"{obj.ToString()} D";
        }

        private void OnCountChanged(int obj)
        {
            _goldText.text = $"{obj.ToString()} G";
        }
        
        private IEnumerator Alert(ValueType type)
        {
            LeanTween.Framework.LeanTween.cancel(_alert.gameObject);
            _alert.gameObject.SetActive(true);
            _alert.alpha = 1;
            _alertText.text = type == ValueType.Gold ? "Not enough <color=white>Gold</color>!" : "Not enough <color=white>Data</color>!";
            LeanTween.Framework.LeanTween.scale(_alert.gameObject, Vector3.one * 1.4f, 0.2f)
                .setEase(LeanTweenType.easeOutBack)
                .setOnComplete(() =>
                {
                    LeanTween.Framework.LeanTween.scale(_alert.gameObject, Vector3.one, 0.1f)
                        .setEase(LeanTweenType.easeInBack);
                });
            yield return new WaitForSeconds(0.6f);
            LeanTween.Framework.LeanTween.value(_alert.gameObject, 1, 0, 0.2f)
                .setOnUpdate(val => _alert.alpha = val)
                .setOnComplete(() => _alert.gameObject.SetActive(false));
            yield return new WaitForSeconds(0.2f);
            var localScale = Vector3.one;
            _alert.transform.localScale = localScale;
        }
    }
}