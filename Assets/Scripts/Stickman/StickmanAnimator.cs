using System;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class StickmanAnimator : MonoBehaviour
{
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

    [SerializeField] private Animator _animator;
    [SerializeField] private string _name;
    private string[] _idleStateNames;
    private string _randomState;

    private void Awake()
    {
        var controller = _animator.runtimeAnimatorController;
        _idleStateNames = controller.animationClips
            .Select(clip => clip.name)
            .ToArray();
    }

    public void Attack()
    {
        _animator.ResetTrigger(IsRunning);
        _animator.SetTrigger(IsAttacking);
    }

    public void Run()
    {
        _animator.ResetTrigger(IsAttacking);
        _animator.SetTrigger(IsRunning);
    }

    public void Idle()
    {
        _animator.ResetTrigger(IsRunning);
        _animator.ResetTrigger(IsAttacking);

        if (String.IsNullOrEmpty(_randomState))
            PlayRandomIdle();
        else
            _animator.Play(_randomState, 0, 0f);
    }

    private void PlayRandomIdle()
    {
        if (_idleStateNames.Length == 0) return;

        _randomState = _idleStateNames[Random.Range(0, _idleStateNames.Length)];
        
        if (_name != String.Empty) // for test
            _randomState = _name;

        _animator.Play(_randomState, 0, 0f);
        Debug.Log(_randomState);
    }
}