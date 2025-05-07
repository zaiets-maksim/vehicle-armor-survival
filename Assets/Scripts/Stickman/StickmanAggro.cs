using UnityEngine;

public class StickmanAggro : MonoBehaviour
{
    [SerializeField] private StickmanAnimator _animatorAnimator;
    
    public void Attack()
    {
        _animatorAnimator.Attack();
    }
}
