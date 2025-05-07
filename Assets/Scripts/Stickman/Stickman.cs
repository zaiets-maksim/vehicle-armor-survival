using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Stickman : MonoBehaviour
{
    [SerializeField] private Car _car; // ZENJECT
    [SerializeField] private int _healthPoints;
    [SerializeField] private Transform _player;
    [SerializeField] private Animator _animator;
    [SerializeField] private RootMotionAgent _rootMotionAgent;
    [SerializeField] private ParticleSystem _damageParticles;
    [SerializeField] private ParticleSystem _deathParticles;
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private StickmanAnimator _stickmanAnimator;
    [SerializeField] private StickmanBehaviour _stickmanBehaviour;
    [SerializeField] private StickmanVision _vision;

    public event Action<int> OnTakeDamage;
    
    private Vector3 _point;

    private void Start()
    {
        _rootMotionAgent.OnDestinationReached += TryChangeToAttackState;
        _vision.StartLooking();
    }

    private void OnDestroy()
    {
        _rootMotionAgent.OnDestinationReached -= TryChangeToAttackState;
    }

    private void Update()
    {
        if (_vision.CanSee && !_vision.CanAttack)
        {
            _stickmanBehaviour.ChangeState<FollowState>();
        }
    }

    private void TryChangeToAttackState() => _stickmanBehaviour.ChangeState<AttackState>();

    public void TakeDamage(Bullet bullet, Vector3 contactPoint)
    {
        _healthPoints -= bullet.Damage;
        OnTakeDamage?.Invoke(bullet.Damage);

        if (_healthPoints <= 0)
        {
            Death();
            return;
        }

        DamageAnimation();
        _damageParticles.transform.SetPositionAndRotation(contactPoint, Quaternion.Inverse(bullet.transform.rotation));
        _damageParticles.gameObject.SetActive(true);
        _damageParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _damageParticles.Play();
    }

    private async void DamageAnimation()
    {
        _skinnedMeshRenderer.material.color = Color.white;
        await UniTask.Delay(250);
        _skinnedMeshRenderer.material.color = Color.red;
    }

    public void Death()
    {
        var pos = transform.position - transform.forward;
        pos.y = 1f;

        _deathParticles.transform.SetPositionAndRotation(pos, Quaternion.identity);
        _deathParticles.transform.SetParent(null);
        _damageParticles.transform.SetParent(null);
        _deathParticles.gameObject.SetActive(true);
        _deathParticles.Play();

        Destroy(_deathParticles.gameObject, 3f);
        Destroy(_damageParticles.gameObject, 3f);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_point, 0.3f);
    }
}