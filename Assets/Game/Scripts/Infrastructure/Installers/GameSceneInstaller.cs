using Game.Scripts.Data.GameScene;
using Game.Scripts.UI.Game;
using UnityEngine;
using Zenject;
using Plane = Game.Scripts.Logic.Plane;

namespace Game.Scripts.Other.Installers
{
    public class GameSceneInstaller: MonoInstaller
    {
        [SerializeField] private UIResults _results;
        [SerializeField] private Plane _plane;
        
        public override void InstallBindings()
        {
            Container.Bind<GameStateData>().AsSingle();
            var plane = Container.InstantiatePrefabForComponent<Plane>(_plane);
            Container.Bind<Plane>().FromInstance(plane).AsSingle();
            Container.Bind<LocalGoldData>().AsSingle();
            var results = Container.InstantiatePrefabForComponent<UIResults>(_results);
            Container.BindInstance(results).AsSingle();
        }
    }
}