using DG.Tweening;
using UnityEngine;

internal class AttackState : StickmanBaseState
{
    private readonly StickmanBehaviour _stickmanBehaviour;
    private readonly StickmanAnimator _animator;
    private StickmanVision _vision;
    private Transform _transform;

    public AttackState(StickmanBehaviour stickmanBehaviour, StickmanAnimator animator, StickmanVision vision, Transform transform)
    {
        _transform = transform;
        _vision = vision;
        _animator = animator;
        _stickmanBehaviour = stickmanBehaviour;
    }

    public override void Enter()
    {
        _animator.Attack();
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        Vector3 direction = _vision.CarNearestPoint - _transform.position;
        _transform.rotation = Quaternion.LookRotation(direction);

        
        // _transform.eulerAngles = new Vector3(0, Quaternion.LookRotation(_vision.CarNearestPoint).eulerAngles.y, 0);
        // rotating to player
    }
}