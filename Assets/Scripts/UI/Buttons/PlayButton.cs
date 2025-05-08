using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Game.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Buttons
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Text _text;
        private IStateMachine<IGameState> _stateMachine;
        
        [Inject]
        public void Constructor(IStateMachine<IGameState> stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private void Start()
        {
            _button.onClick.AddListener(Play);
        }
        
        public void Play()
        {
            _stateMachine.Enter<LoadLevelState, string>("Gameplay");
        }
    }
}