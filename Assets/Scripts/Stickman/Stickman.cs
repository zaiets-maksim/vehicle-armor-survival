using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using tetris.Scripts.Extensions;
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
    [SerializeField] private DamageView _damageView;
    [SerializeField] private HealthView _healthView;

    public event Action<int> OnTakeDamage;

    private IGameFactory _gameFactory;

    private Player _player;
    private Vector3 _point;
    private IGameCurator _gameCurator;


    [Inject]
    public void Constructor(IGameFactory gameFactory, IGameCurator gameCurator)
    {
        _gameCurator = gameCurator;
        _gameFactory = gameFactory;

        _player = _gameFactory.Player;
    }

    private void Start()
    {
        _rootMotionAgent.OnDestinationReached += TryChangeToAttackState;
        _gameCurator.OnEndGame += Disable;
        Init();
    }

    private void OnDestroy()
    {
        _rootMotionAgent.OnDestinationReached -= TryChangeToAttackState;
        _gameCurator.OnEndGame -= Disable;
    }

    private void Disable(GameResult result)
    {
        _vision.Disable();
        _rootMotionAgent.SetDestination(transform.position);
        _stickmanBehaviour.ChangeState<IdleState>();
        enabled = false;
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
        _healthView.Init(_health, this);
    }

    private void TryChangeToAttackState() => _stickmanBehaviour.ChangeState<AttackState>();

    public override void TakeDamage(Bullet bullet, Vector3 contactPoint)
    {
        _health -= bullet.Damage;
        OnTakeDamage?.Invoke(bullet.Damage);
        _damageView.ShowDamage(bullet.Damage);

        if (_health <= 0)
        {
            _damageView.ShowDamage(bullet.Damage);
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
        await UniTask.Delay(0.25f.ToMiliseconds());
        if (_health <= 0)
            return;

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
        transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => { gameObject.SetActive(false); });
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_point, 0.3f);
    }
}