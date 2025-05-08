using Connect4.Scripts.Infrastructure;
using PiratesIdle.Scripts.Infrastructure;
using Services.DataStorageService;
using Services.EnemyGenerator;
using Services.Factories.UIFactory;
using Services.StaticDataService;
using Services.WindowService;
using StaticData.Levels;
using UnityEngine;
using Zenject;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadLevelState : IPayloadedState<string>, IGameState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IUIFactory _uiFactory;
        private readonly IWindowService _windowService;
        private readonly IPersistenceProgressService _progress;
        private readonly IGameFactory _gameFactory;
        private readonly LevelStaticData _levelStaticData;
        private readonly IEnemyFactory _enemyFactory;
        private readonly IEnemyGenerator _enemyGenerator;

        [Inject]
        public LoadLevelState(IStateMachine<IGameState> gameStateMachine, ISceneLoader sceneLoader,
            ILoadingCurtain loadingCurtain, IUIFactory uiFactory, IWindowService windowService, IPersistenceProgressService progress,
            IGameFactory gameFactory, IStaticDataService staticDataService, IEnemyFactory enemyFactory, IEnemyGenerator enemyGenerator)
        {
            _enemyGenerator = enemyGenerator;
            _enemyFactory = enemyFactory;
            _gameFactory = gameFactory;
            _progress = progress;
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _uiFactory = uiFactory;
            _windowService = windowService;
            _levelStaticData = staticDataService.LevelConfig();
        }
        
        public void Enter(string payload)
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(payload, OnLevelLoad);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        protected virtual void OnLevelLoad()
        {
            _windowService.Close();
            _uiFactory.CreateHud();
            InitGameWorld();
            _gameStateMachine.Enter<GameLoopState>();
        }

        private void InitGameWorld()
        {
            var camera = _gameFactory.CreateCamera();
            var input = _gameFactory.CreateInputController();
            var player = _gameFactory.CreatePlayer();
            player.Initialize(input, camera.GetComponent<Camera>());
            camera.Initialize(player);
            
            // there are 2 methods of spawn (just from example)
            foreach (var enemyData in _levelStaticData.EnemyData) 
                _enemyFactory.CreateEnemy(enemyData.TypeId, enemyData.Position, enemyData.Rotation);
            
            _enemyGenerator.Generate();
        }
    }
}