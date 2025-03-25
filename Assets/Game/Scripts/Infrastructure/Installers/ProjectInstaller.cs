using Game.Scripts.Ads;
using Game.Scripts.Data.Config;
using Game.Scripts.Data.Project;
using Game.Scripts.Logic;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Other.Installers
{
    public class ProjectInstaller: MonoInstaller
    {
        [SerializeField] private SoundManager _soundManager;
        [SerializeField] private SOPlane[] _planes;
        [SerializeField] private SOMission[] _missions;
        [SerializeField] private SOUpgrade[] _upgrades;
        
        public override void InstallBindings()
        {
            Container.Bind<DiContainer>().FromInstance(Container).AsSingle();
            Container.Bind<AudioSettingsData>().AsSingle();
            var soundManager = Container.InstantiatePrefabForComponent<SoundManager>(_soundManager);
            Container.BindInstance(soundManager).AsSingle();
            Container.Bind<GoldData>().AsSingle();
            var planes = new PlaneData[_planes.Length];
            for (var i = 0; i < _planes.Length; i++)
            {
                var plane = _planes[i];
                planes[i] = new PlaneData(plane);
            }
            Container.Bind<PlaneData[]>().FromInstance(planes).AsSingle();
            var missions = new MissionData[_missions.Length];
            for (var i1 = 0; i1 < _missions.Length; i1++)
            {
                var mission = _missions[i1];
                missions[i1] = new MissionData(mission);
            }
            Container.Bind<MissionData[]>().FromInstance(missions).AsSingle();
            Container.Bind<LevelData>().AsSingle();
            Container.Bind<DataData>().AsSingle();
            var handler = new GameObject("InputHandler");
            handler.AddComponent<PlayerControlHandler>();
            Container.InjectGameObject(handler);
            Container.BindInstance(handler.GetComponent<PlayerControlHandler>()).AsSingle();
            var upgradesData = new UpgradesData(_upgrades);
            Container.BindInstance(upgradesData).AsSingle();
            var gameObject = new GameObject("AdMobAds");
            gameObject.AddComponent<AdMobAds>();
            Container.BindInstance(gameObject.GetComponent<AdMobAds>()).AsSingle().NonLazy();
        }
    }
}