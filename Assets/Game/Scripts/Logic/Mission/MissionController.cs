using Cinemachine;
using Game.Scripts.Data.Project;
using Game.Scripts.Other;
using LeanTween.Framework;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Logic.Mission
{
    public class MissionController: MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TMP_Text _timer;
        [SerializeField] private AudioClip _timerSound;
        [SerializeField] private AudioClip _effectSound;
        
        private SoundManager _soundManager;
        private MissionData _missionData;
        private Levels.Mission _mission;
        private DiContainer _container;
        private LevelData _levelData;
        private bool _timerSoundPlayed;
        private Plane _plane;
        
        [Inject]
        private void Construct(DiContainer container, Plane plane, LevelData levelData, SoundManager soundManager)
        {
            _container = container;
            _plane = plane;
            _levelData = levelData;
            _soundManager = soundManager;
            _soundManager.PlayMusic();
        }
        
        private void Start()
        {
            _camera.Follow = _plane.transform;
            var go = _container.InstantiatePrefab(_levelData.MissionData.Mission.Mission);
            _mission = go.GetComponent<Levels.Mission>();
            _mission.OnUIUpdate += UpdateUI;
            _mission.OnTimerUpdate += UpdateTimer;
        }

        private void OnDestroy()
        {
            _mission.OnUIUpdate -= UpdateUI;
            _mission.OnTimerUpdate -= UpdateTimer;
        }

        private void UpdateUI(string obj)
        {
            _text.text = obj;
            _soundManager.PlaySound(_effectSound);
            LeanTween.Framework.LeanTween.scale(_text.gameObject, Vector3.one * 1.2f, 0.1f)
                .setEase(LeanTweenType.easeOutBack)
                .setOnComplete(() =>
                {
                    LeanTween.Framework.LeanTween.scale(_text.gameObject, Vector3.one, 0.1f)
                        .setEase(LeanTweenType.easeInBack);
                });
        }
        
        private void UpdateTimer(float time)
        {
            _timer.text = $"{time:F0}\nsec";
            if (!(time <= 5f) || _timerSoundPlayed) return;
            _soundManager.PlaySound(_timerSound);
            _timerSoundPlayed = true;
        }
    }
}