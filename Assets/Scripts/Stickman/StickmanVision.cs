using System.Collections;
using UnityEngine;
using Zenject;

public class StickmanVision : MonoBehaviour
{
    [SerializeField] private int _radius;
    [SerializeField] private int _radiusForAttack;
    [SerializeField] private float _checkInterval = 0.4f;
    
    private Player _player;

    private bool _canSee;
    private Coroutine _lookingCoroutine;
    private float _distanceToPlayer;
    private bool _canAttack;
    private Vector3 _playerNearestPoint;
    private IGameFactory _gameFactory;

    public Vector3 PlayerNearestPoint => _playerNearestPoint;
    public float DistanceToPlayer => _distanceToPlayer;
    
    public double VisionRadius => _radius;
    public bool CanSee => _canSee;
    public bool CanAttack => _canAttack;

    [Inject]
    public void Constructor(IGameFactory gameFactory)
    {
        _gameFactory = gameFactory;

        _player = _gameFactory.Player;
    }

    public void StartLooking()
    {
        _lookingCoroutine ??= StartCoroutine(Looking());
    }

    public void StopLooking()
    {
        if(_lookingCoroutine != null)
            StopCoroutine(_lookingCoroutine);
    }

    public float GetDistanceTo(Vector3 point) => Vector3.Distance(transform.position, point);
    

    private IEnumerator Looking()
    {
        while (true)
        {
            _playerNearestPoint = _player.GetNearestPointTo(transform.position);
            _distanceToPlayer = GetDistanceTo(_playerNearestPoint);
            _canSee = _distanceToPlayer < _radius;

            if (_canSee)
                _canAttack = _distanceToPlayer < _radiusForAttack;
            
            // if(_canAttack)
            //     StopLooking();
            
            yield return new WaitForSeconds(_checkInterval);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 1f, 0.25f);
        Gizmos.DrawSphere(transform.position, _radius);
        
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        Gizmos.DrawSphere(transform.position, _radiusForAttack);
    }

    public void Disable()
    {
        StopLooking();
        _canAttack = false;
        _canSee = false;
    }
}
