using System.Collections;
using UnityEngine;

public class StickmanVision : MonoBehaviour
{
    [SerializeField] private int _radius;
    [SerializeField] private int _radiusForAttack;
    [SerializeField] private float _checkInterval = 0.4f;
    
    [SerializeField] private Car _car; // ZENJECT

    private bool _canSee;
    private Coroutine _lookingCoroutine;
    private float _distanceToCar;
    private bool _canAttack;
    private Vector3 _carNearestPoint;

    public Vector3 CarNearestPoint => _carNearestPoint;
    public float DistanceToCar => _distanceToCar;
    
    public double VisionRadius => _radius;
    public bool CanSee => _canSee;
    public bool CanAttack => _canAttack;

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
            _carNearestPoint = _car.GetNearestPointTo(transform.position);
            _distanceToCar = GetDistanceTo(_carNearestPoint);
            _canSee = _distanceToCar < _radius;

            if (_canSee)
                _canAttack = _distanceToCar < _radiusForAttack;
            
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
}
