using Connect4.Scripts.Infrastructure;
using PiratesIdle.Scripts.Infrastructure;
using Services.DataStorageService;
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
        private IGameFactory _gameFactory;
        private LevelStaticData _levelStaticData;
        private IStaticDataService _staticDataService;
        private IEnemyFactory _enemyFactory;

        [Inject]
        public LoadLevelState(IStateMachine<IGameState> gameStateMachine, ISceneLoader sceneLoader,
            ILoadingCurtain loadingCurtain, IUIFactory uiFactory, IWindowService windowService, IPersistenceProgressService progress,
            IGameFactory gameFactory, IStaticDataService staticDataService, IEnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
            _staticDataService = staticDataService;
            _gameFactory = gameFactory;
            _progress = progress;
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _uiFactory = uiFactory;
            _windowService = windowService;
            _levelStaticData = _staticDataService.LevelConfig();
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
            
            foreach (var enemyData in _levelStaticData.EnemyData)
            {
                _enemyFactory.CreateEnemy(enemyData.TypeId, enemyData.Position, enemyData.Rotation);
            }
        }
    }
}