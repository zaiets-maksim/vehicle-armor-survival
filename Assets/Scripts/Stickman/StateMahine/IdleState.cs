internal class IdleState : StickmanBaseState
{
    private readonly StickmanAnimator _personAnimator;
    private StickmanBehaviour _stickmanBehaviour;

    public IdleState(StickmanBehaviour stickmanBehaviour, StickmanAnimator personAnimator)
    {
        _stickmanBehaviour = stickmanBehaviour;
        _personAnimator = personAnimator;
    }
        
    public override async void Enter()
    {
        _personAnimator.Idle();
    }

    public override void Exit()
    {
            
    }
}