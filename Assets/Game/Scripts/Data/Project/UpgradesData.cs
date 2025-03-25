using System.Collections.Generic;
using Game.Scripts.Data.Config;
using Game.Scripts.Data.Enums;
using UnityEngine;

namespace Game.Scripts.Data.Project
{
    public class UpgradesData
    {
        private readonly SOUpgrade[] _upgrades;

        public Dictionary<UpgradeType, int> UpgradesLvl { get; } = new ();

        public SOUpgrade GetUpgrade(UpgradeType type)
        {
            foreach (var upgrade in _upgrades)
                if (upgrade.Type == type)
                    return upgrade;
            return null;
        }
        
        public bool IsMaxLvl(UpgradeType type)
        {
            foreach (var upgrade in _upgrades)
                if (upgrade.Type == type)
                    return UpgradesLvl[type] >= upgrade.MaxLvl;
            return false;
        }

        public float GetValue(UpgradeType type)
        {
            foreach (var upgrade in _upgrades)
            {
                if (upgrade.Type == type)
                {
                    return upgrade.BaseValue * UpgradesLvl[type];
                }
            }
            return 0;
        }
        
        public void IncreaseUpgradeLvl(UpgradeType type)
        {
            var maxLvl = false;
            foreach (var upgrade in _upgrades)
                if (upgrade.Type == type)
                    if (UpgradesLvl[type] >= upgrade.MaxLvl)
                        maxLvl = true;
            if(maxLvl) return;
            UpgradesLvl[type] += 1;
            PlayerPrefs.SetInt(type.ToString(), UpgradesLvl[type]);
        }
        
        public int GetCost(UpgradeType type)
        {
            foreach (var upgrade in _upgrades)
                if (upgrade.Type == type)
                    return upgrade.UpgradeCost * (UpgradesLvl[type] + 1);
            return 0;
        }

        public UpgradesData(SOUpgrade[] upgrades)
        {
            _upgrades = upgrades;
            foreach (var upgrade in _upgrades)
            {
                var value = PlayerPrefs.GetInt(upgrade.Type.ToString(), 0);
                UpgradesLvl.Add(upgrade.Type, value);
            }
        }
    }
}