using System;
using System.Collections;
using System.Linq;
using Game.Scripts.Data.Project;
using Game.Scripts.Logic.Mission;
using Game.Scripts.Other;
using Game.Scripts.UI.Menu.Planes;
using LeanTween.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.UI.Menu.Missions
{
    public class UIMissionsPanel: UIPanelTemplate
    {
        [Header("Mission")]
        [SerializeField] private UIMission[] _missions;
        [SerializeField] private Button[] _types;
        [Header("Planes")] 
        [SerializeField] private UIPlane[] _planes;
        [Header("Other")]
        [SerializeField] private TMP_Text _dataText;
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private GameObject _missionPanel;
        [SerializeField] private GameObject _planePanel;
        [SerializeField] private TMP_Text _alertText;
        [SerializeField] private CanvasGroup _alert;

        private MissionType _missionType;
        private MissionData[] _missionData;
        private PlaneData[] _planesData;
        private LevelData _levelData;
        private GoldData _goldData;
        private DataData _dataData;
        
        [Inject]
        private void Construct(LevelData levelData, DataData dataData,
            MissionData[] missionData, PlaneData[] planesData, GoldData goldData)
        {
            _levelData = levelData;
            _dataData = dataData;
            _missionData = missionData;
            _planesData = planesData;
            _goldData = goldData;
        }
        
        protected override void Awake()
        {
            base.Awake();
            _types[0].interactable = false;
            _types[0].onClick.AddListener(() => {OnTypeClick(MissionType.Collection);});
            _types[1].onClick.AddListener(() => {OnTypeClick(MissionType.Exploration);});
            _types[2].onClick.AddListener(() => {OnTypeClick(MissionType.Protection);});
            _dataData.OnDataCountChanged += OnDataCountChanged;
            _goldData.OnCountChanged += OnCountChanged;
            _goldText.text = $"{_goldData.Count.ToString()} G";
            _dataText.text = $"{_dataData.Data.ToString()} D";
        }

        private void OnCountChanged(int obj)
        {
            _goldText.text = $"{obj.ToString()} G";
        }

        private void OnDataCountChanged(int obj)
        {
            _dataText.text = $"{obj.ToString()} D";
        }

        private void Start()
        {
            _missionType = MissionType.Collection;
            SetMissions();
        }
        
        public override void Show()
        {
            base.Show();
            _missionPanel.SetActive(true);
            _planePanel.SetActive(false);
        }

        private void OnTypeClick(MissionType missionType)
        {
            _missionType = missionType;
            foreach (var type in _types)
                type.interactable = true;
            switch (missionType)
            {
                case MissionType.Collection:
                    _types[0].interactable = false;
                    break;
                case MissionType.Exploration:
                    _types[1].interactable = false;
                    break;
                case MissionType.Protection:
                    _types[2].interactable = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(missionType), missionType, null);
            }
            SetMissions();
        }

        private void SetMissions()
        {
            var missions = GetCurrentMissions();
            for(int i = 0; i < _missions.Length; i++)
            {
                _missions[i].Init(missions[i]);
                var index = i;
                _missions[i].Btn.onClick.RemoveAllListeners();
                _missions[i].Btn.onClick.AddListener(() => ChooseMission(missions[index]));
            }
        }

        private MissionData[] GetCurrentMissions() =>
            _missionData.Where(mission => mission.Mission.MissionType == _missionType).ToArray();

        private void ChooseMission(MissionData missionData)
        {
            if (missionData.State == State.Locked)
            {
                if (_dataData.Data < missionData.Mission.DataPrice)
                {
                    StopAllCoroutines();
                    StartCoroutine(Alert());
                    return;
                }
                _dataData.Data -= missionData.Mission.DataPrice;
                missionData.State = State.Unlocked;
                SetMissions();
            }
            else if (missionData.State == State.Unlocked)
            {
                _levelData.MissionData = missionData;
                _missionPanel.SetActive(false);
                _planePanel.SetActive(true);
                SetPlanes();
            }
        }

        private void SetPlanes()
        {
            var planes = GetCurrentMissionPlanes();
            for (var i = 0; i < _planes.Length; i++)
            {
                _planes[i].MissionInit(planes[i]);
                var index = i;
                _planes[i].BuyButton.onClick.RemoveAllListeners();
                _planes[i].BuyButton.interactable = planes[i].State == State.Unlocked;
                _planes[i].BuyButton.onClick.AddListener(() => OnPlaneSelect(planes[index]));
            } 
        }

        private void OnPlaneSelect(PlaneData plane)
        {
            if(plane.State != State.Unlocked) return;
            _levelData.PlaneData = plane;
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }

        private PlaneData[] GetCurrentMissionPlanes() => 
            _planesData.Where(plane => plane.Plane.MissionType == _missionType).ToArray();
        
        private IEnumerator Alert()
        {
            LeanTween.Framework.LeanTween.cancel(_alert.gameObject);
            _alert.gameObject.SetActive(true);
            _alert.alpha = 1;
            _alertText.text = "Not enough <color=white>Data</color>!";
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