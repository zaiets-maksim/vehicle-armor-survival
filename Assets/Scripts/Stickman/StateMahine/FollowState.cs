using UnityEngine;

internal class FollowState : StickmanBaseState
{
    private readonly StickmanBehaviour _stickmanBehaviour;
    private readonly StickmanAnimator _animator;
    private readonly RootMotionAgent _rootMotionAgent;
    private readonly Transform _transform;
    private readonly Car _player;
    private readonly Stickman _stickman;
    
    private Vector3 _point;

    public FollowState(StickmanBehaviour stickmanBehaviour, StickmanAnimator animator, RootMotionAgent rootMotionAgent, Stickman stickman, Car player)
    {
        _stickman = stickman;
        _transform = _stickman.transform;
        _player = player;
        _rootMotionAgent = rootMotionAgent;
        _animator = animator;
        _stickmanBehaviour = stickmanBehaviour;
    }

    public override void Enter()
    {
        _animator.Run();
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        _point = _player.GetNearestPointTo(_transform.position);
        _rootMotionAgent.SetDestination(_point);
    }
}