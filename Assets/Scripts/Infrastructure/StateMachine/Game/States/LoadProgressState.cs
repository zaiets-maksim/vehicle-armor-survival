using Services.DataStorageService;
using Services.SaveLoad;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadProgressState : IPayloadedState<string>, IGameState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly IPersistenceProgressService _progress;
        private readonly ISaveLoadService _saveLoad;

        public LoadProgressState(IStateMachine<IGameState> stateMachine, IPersistenceProgressService progress, 
            ISaveLoadService saveLoad)
        {
            _saveLoad = saveLoad;
            _progress = progress;
            _stateMachine = stateMachine;
        }

        public void Enter(string payload)
        {
            LoadOrCreatePlayerData();
            _saveLoad.SaveProgress();
            _stateMachine.Enter<MenuLevelState, string>("MainMenu");
        }
        
        private PlayerData LoadOrCreatePlayerData() =>
            _progress.PlayerData =
                _saveLoad.LoadProgress()
                ?? CreateNew();

        private PlayerData CreateNew()
        {
            PlayerData playerData = new PlayerData
            {
                
            };
            return playerData;
        }

        public void Exit()
        {
            
        }
    }
}