using UnityEngine;
using Zenject;

[RequireComponent(typeof(Stickman))]
[RequireComponent(typeof(RootMotionAgent))]
[RequireComponent(typeof(StickmanAnimator))]
public class StickmanBehaviour : StickmanStateMachine
{
    [SerializeField] private Stickman _stickman;
    [SerializeField] private StickmanAnimator _animator;
    [SerializeField] private RootMotionAgent _rootMotionAgent;
    [SerializeField] private StickmanVision _stickmanVision;
    
    
    private Player _player;
    private IGameFactory _gameFactory;

    [Inject]
    public void Constructor(IGameFactory gameFactory)
    {
        _gameFactory = gameFactory;

        _player = _gameFactory.Player;
    }

    private void OnEnable()
    {
        _states = CreateStates(
            new IdleState(this, _animator),
            new FollowState(this, _animator, _rootMotionAgent, _stickman, _player),
            new AttackState(this, _animator, _stickmanVision, transform)
        );

        ChangeState<IdleState>();
    }

    private void Update() => _currentState?.Update();
}