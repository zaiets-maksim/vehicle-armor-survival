using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using tetris.Scripts.Extensions;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour, IDamageble
{
    [SerializeField] private int _health;
    [SerializeField] private Collider _collider;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private PlayerDeath _playerDeath;
    [SerializeField] private Turret _turret;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float noiseStrength = 1f;
    [SerializeField] private float noiseSampleSpeed = 0.5f;
    [SerializeField] private float maxXRadius = 5f;
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private HealthView _healthView;

    public event Action<int> OnTakeDamage;

    private float _noiseOffset;
    private Vector3 _currentVelocity;
    private Vector3 _lastPosition;
    private Vector3 _closestPoint;
    private Coroutine _perlinMotionCoroutine;
    private Material _material;
    private static readonly int OverrideAmount = Shader.PropertyToID("_OverrideAmount");
    private IGameCurator _gameCurator;

    public Vector3 CurrentVelocity => _currentVelocity;
    public int Health => _health;
    public bool IsAlive => _health > 0;

    [Inject]
    public void Constructor(IGameCurator gameCurator)
    {
        _gameCurator = gameCurator;
    }

    private void Start()
    {
        _material = _meshRenderer.material;
        _noiseOffset = Random.Range(0f, 100f);
        _gameCurator.OnStartGame += Enable;
        _healthView.Init(_health, this);
    }

    private void OnDestroy()
    {
        _gameCurator.OnStartGame -= Enable;
    }

    public void Enable()
    {
        _turret.Enable();
        _perlinMotionCoroutine = StartCoroutine(SimulatePerlinMotion());
        _healthBar.SetActive(true);
    }

    public async void Disable(float stopDelay = 0f)
    {
        _turret.Disable();
        _healthBar.SetActive(false);

        if (_perlinMotionCoroutine != null)
        {
            await UniTask.Delay(stopDelay.ToMiliseconds());
            StopCoroutine(_perlinMotionCoroutine);
        }
    }

    private void SmoothStop(float duration) => DOTween.To(() => moveSpeed, x => moveSpeed = x, 0f, duration);

    public async void TakeDamage(int damage)
    {
        if(!IsAlive)
            return;
        
        _health -= damage;
        OnTakeDamage?.Invoke(damage);

        AnimateDamage();

        if (_health <= 0)
        {
            await _playerDeath.Active();
            _gameCurator.EndGame(GameResult.Lose);
        }
    }

    public void TryWin()
    {
        if(!IsAlive)
            return;
        
        _gameCurator.EndGame(GameResult.Win);
        SmoothStop(1f);
        Disable(1f);
    }

    private void AnimateDamage()
    {
        DOTween.To(
            () => _material.GetFloat(OverrideAmount),
            x => _material.SetFloat(OverrideAmount, x),
            1f,
            0.1f
        ).OnComplete(() =>
        {
            DOTween.To(
                () => _material.GetFloat(OverrideAmount),
                x => _material.SetFloat(OverrideAmount, x),
                0f,
                0.15f);
        });

        transform.DOPunchScale(Vector3.one * 0.1f, 0.3f);
    }

    public void Initialize(InputController input, Camera camera)
    {
        _turret.Initialize(input, camera);
    }

    public Vector3 GetNearestPointTo(Vector3 position)
    {
        _closestPoint = _collider.ClosestPoint(position);
        return _closestPoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-maxXRadius, 0, transform.position.z - 7),
            new Vector3(-maxXRadius, 0, transform.position.z + 7));
        Gizmos.DrawLine(new Vector3(maxXRadius, 0, transform.position.z - 7),
            new Vector3(maxXRadius, 0, transform.position.z + 7));

        Gizmos.DrawSphere(_closestPoint, 0.2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(-maxXRadius * 0.8f, 0, transform.position.z - 7),
            new Vector3(-maxXRadius * 0.8f, 0, transform.position.z + 7));
        Gizmos.DrawLine(new Vector3(maxXRadius * 0.8f, 0, transform.position.z - 7),
            new Vector3(maxXRadius * 0.8f, 0, transform.position.z + 7));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(-maxXRadius * 0.5f, 0, transform.position.z - 7),
            new Vector3(-maxXRadius * 0.5f, 0, transform.position.z + 7));
        Gizmos.DrawLine(new Vector3(maxXRadius * 0.5f, 0, transform.position.z - 7),
            new Vector3(maxXRadius * 0.5f, 0, transform.position.z + 7));
    }

    private IEnumerator SimulatePerlinMotion()
    {
        while (true)
        {
            _currentVelocity = (transform.position - _lastPosition) / Time.fixedDeltaTime;
            _lastPosition = transform.position;

            transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));

            float perlinValue = Mathf.PerlinNoise(_noiseOffset, Time.time * noiseSampleSpeed);

            float turnDirection = (perlinValue * 2f - 1f) * noiseStrength;

            float distanceFromCenter = Mathf.Abs(transform.position.x);
            float distanceRatio = distanceFromCenter / maxXRadius;

            float directionToCenter = transform.position.x > 0 ? -1f : 1f;

            if (distanceRatio > 0)
            {
                turnDirection *= (1f - distanceRatio * 0.9f);

                if (Mathf.Sign(turnDirection) != directionToCenter)
                {
                    turnDirection *= (1f - distanceRatio);

                    if (distanceRatio > 0.8f)
                    {
                        turnDirection += directionToCenter * (distanceRatio - 0.8f) * 5f;
                    }
                }
            }

            transform.Rotate(0f, turnDirection * rotationSpeed * Time.deltaTime, 0f);

            if (Mathf.Abs(transform.position.x) > maxXRadius)
            {
                Vector3 clampedPosition = transform.position;
                clampedPosition.x = Mathf.Clamp(clampedPosition.x, -maxXRadius, maxXRadius);
                transform.position = clampedPosition;

                float forcedTurn = transform.position.x > 0 ? -1f : 1f;
                transform.Rotate(0f, forcedTurn * rotationSpeed * Time.deltaTime * 2f, 0f);
            }

            _noiseOffset += noiseSampleSpeed * Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }
    }
}