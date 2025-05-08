using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Stickman : Enemy, IDamageble
{
    [SerializeField] private Animator _animator;
    [SerializeField] private RootMotionAgent _rootMotionAgent;
    [SerializeField] private ParticleSystem _damageParticles;
    [SerializeField] private ParticleSystem _deathParticles;
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private StickmanAnimator _stickmanAnimator;
    [SerializeField] private StickmanBehaviour _stickmanBehaviour;
    [SerializeField] private StickmanVision _vision;
    
    public event Action<int> OnTakeDamage;
    
    private IGameFactory _gameFactory;
    
    private Player _player;
    private Vector3 _point;


    [Inject]
    public void Constructor(IGameFactory gameFactory)
    {
        _gameFactory = gameFactory;
        
        _player = _gameFactory.Player;
    }

    private void Start()
    {
        _rootMotionAgent.OnDestinationReached += TryChangeToAttackState;
        Init();
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
    
    private void Init()
    {
        _vision.StartLooking();
    }

    private void TryChangeToAttackState() => _stickmanBehaviour.ChangeState<AttackState>();

    public override void TakeDamage(Bullet bullet, Vector3 contactPoint)
    {
        _health -= bullet.Damage;
        OnTakeDamage?.Invoke(bullet.Damage);

        if (_health <= 0)
        {
            Death();
            return;
        }
        
        AnimateDamage();
        _damageParticles.transform.SetPositionAndRotation(contactPoint, Quaternion.Inverse(bullet.transform.rotation));
        _damageParticles.gameObject.SetActive(true);
        _damageParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _damageParticles.Play();
    }

    private async void AnimateDamage()
    {
        transform.DOShakePosition(0.3f, 0.2f);
        transform.DOShakeRotation(0.3f, 50f);
        transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
        
        _skinnedMeshRenderer.material.color = Color.white;
        await UniTask.Delay(250);
        _skinnedMeshRenderer.material.color = Color.red;
    }

    public override void Death()
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