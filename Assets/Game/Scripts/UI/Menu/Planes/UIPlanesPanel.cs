using System;
using System.Collections;
using System.Linq;
using Game.Scripts.Data.Project;
using Game.Scripts.Logic.Mission;
using Game.Scripts.Other;
using LeanTween.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.UI.Menu.Planes
{
    public class UIPlanesPanel: UIPanelTemplate
    {
        [SerializeField] private Button _nextType;
        [SerializeField] private Button _prevType;
        [SerializeField] private TMP_Text _typeText;
        [SerializeField] private CanvasGroup _alert;
        [SerializeField] private TMP_Text _alertText;
        [SerializeField] private UIPlane[] _planes;
        [SerializeField] private TMP_Text _goldText;

        private MissionType _missionType;
        private PlaneData[] _planesData;
        private GoldData _goldData;
        private bool _firstTime = true;
        
        [Inject]
        private void Construct(PlaneData[] planesData, GoldData goldData)
        {
            _planesData = planesData;
            _goldData = goldData;
        }

        protected override void Awake()
        {
            base.Awake();
            _nextType.onClick.AddListener(SetNextType);
            _prevType.onClick.AddListener(SetPrevType);
            _goldData.OnCountChanged += OnCountChanged;
            _goldText.text = $"{_goldData.Count.ToString()} G";
        }

        private void Start()
        {
            _missionType = MissionType.Collection;
            SetPlanes();
        }

        private void OnCountChanged(int obj)
        {
            _goldText.text = $"{obj.ToString()} G";
        }

        private void SetPrevType()
        {
            _missionType = _missionType switch
            {
                MissionType.Exploration => MissionType.Protection,
                MissionType.Collection => MissionType.Exploration,
                MissionType.Protection => MissionType.Collection,
                _ => throw new ArgumentOutOfRangeException()
            };
            SetPlanes();
        }

        private void SetNextType()
        {
            _missionType = _missionType switch
            {
                MissionType.Exploration => MissionType.Collection,
                MissionType.Collection => MissionType.Protection,//
                MissionType.Protection => MissionType.Exploration,
                _ => throw new ArgumentOutOfRangeException()
            };
            SetPlanes();
        }

        private void SetPlanes()
        {
            var planes = GetCurrentMissionPlanes();
            _typeText.text = _missionType switch
            {
                MissionType.Exploration => "Collection",
                MissionType.Collection => "Exploration",
                MissionType.Protection => "Race",
                _ => throw new ArgumentOutOfRangeException()
            };;
            for (var i = 0; i < _planes.Length; i++)
            {
                _planes[i].Init(planes[i]);
                var index = i;
                _planes[i].BuyButton.onClick.RemoveAllListeners();
                _planes[i].BuyButton.onClick.AddListener(() => OnBuyClick(planes[index]));
            } 
        }

        private PlaneData[] GetCurrentMissionPlanes() => 
            _planesData.Where(plane => plane.Plane.MissionType == _missionType).ToArray();

        private void OnBuyClick(PlaneData plane)
        {
            if (plane.State == State.Locked)
            {
                if(_goldData.Count < plane.Plane.Price)
                {
                    StopAllCoroutines();
                    StartCoroutine(Alert());
                    return;
                }
                _goldData.Count -= plane.Plane.Price;
                plane.State = State.Unlocked;
            }
            else if(plane.State == State.Unlocked)
            {
                if(_goldData.Count < plane.UpgradePrice)
                {
                    StopAllCoroutines();
                    StartCoroutine(Alert());
                    return;
                }
                _goldData.Count -= plane.UpgradePrice;
                plane.Lvl++;
            }
            SetPlanes();
        }
        
        private IEnumerator Alert()
        {
            LeanTween.Framework.LeanTween.cancel(_alert.gameObject);
            _alert.gameObject.SetActive(true);
            _alert.alpha = 1;
            _alertText.text = "Not enough <color=white>Gold</color>!";
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