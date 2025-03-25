using System;
using Game.Scripts.Data.Project;
using Game.Scripts.Logic.Mission;
using Game.Scripts.Other;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Menu.Planes
{
    public class UIPlane: MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _type;
        [SerializeField] private TMP_Text _maxSpeed;
        [SerializeField] private TMP_Text _acceleration;
        [SerializeField] private TMP_Text _controllability;
        [SerializeField] private TMP_Text _buttonText;
        [SerializeField] private TMP_Text _lvl;
        [SerializeField] private Button _buyButton;

        public Button BuyButton => _buyButton;
        
        public void Init(PlaneData planeData)
        {
            _icon.sprite = planeData.Plane.Icon;
            _name.text = planeData.Plane.Name;
            _type.text = planeData.Plane.MissionType switch
            {
                MissionType.Collection => "Jet",
                MissionType.Exploration => "Plane",
                MissionType.Protection => "Sport",
                _ => throw new ArgumentOutOfRangeException()
            };
            _maxSpeed.text = $"Max speed: <color=white>{planeData.MaxSpeed * 100}</color>";
            _acceleration.text = $"Acceleration: <color=white>{planeData.Acceleration * 100}</color>";
            _controllability.text = $"Controllability: <color=white>{planeData.Controllability * 100}</color>";
            _lvl.text = $"lv. <color=white>{planeData.Lvl + 1}</color>";
            _buttonText.text = planeData.State switch
            {
                State.Locked => $"Buy <color=white>\n{planeData.Plane.Price}</color> g",
                State.Unlocked => $"Upgrade <color=white>\n{planeData.UpgradePrice}</color> g",//
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void MissionInit(PlaneData planeData)
        {
            _icon.sprite = planeData.Plane.Icon;
            _name.text = planeData.Plane.Name;
            _type.text = planeData.Plane.MissionType switch
            {
                MissionType.Collection => "Jet",
                MissionType.Exploration => "Plane",
                MissionType.Protection => "Sport",
                _ => throw new ArgumentOutOfRangeException()
            };
            _maxSpeed.text = $"Max speed: <color=white>{planeData.MaxSpeed * 100}</color>";
            _acceleration.text = $"Acceleration: <color=white>{planeData.Acceleration * 100}</color>";
            _controllability.text = $"Controllability: <color=white>{planeData.Controllability * 100}</color>";
            _lvl.text = $"lv. <color=white>{planeData.Lvl+1}</color>";
            _buttonText.text = planeData.State switch
            {
                State.Locked => "Locked",
                State.Unlocked => "Select",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}