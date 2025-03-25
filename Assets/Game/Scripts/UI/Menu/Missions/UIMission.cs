using Game.Scripts.Data.Project;
using Game.Scripts.Other;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Gradient = Tools.Gradient;

namespace Game.Scripts.UI.Menu.Missions
{
    public class UIMission: MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _btnText;
        [SerializeField] private Button _btn;
        
        public Button Btn => _btn;
        
        public void Init(MissionData missionData)
        {
            _name.text = missionData.Mission.Difficulty.ToString();//
            _icon.sprite = missionData.Mission.Icon;
            _btnText.text = missionData.State switch
            {
                State.Locked => $"{missionData.Mission.DataPrice} d",
                State.Unlocked => "Select",
                _ => throw new System.ArgumentOutOfRangeException()
            };
        }
    }
}