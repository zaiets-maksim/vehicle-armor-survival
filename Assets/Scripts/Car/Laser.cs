using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _length;

    private void Awake()
    {
        _lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        Vector3 start = _firePoint.position;
        Vector3 end = start + _firePoint.forward * _length;
        
        // if (Physics.Raycast(start, _firePoint.forward, out RaycastHit hit, _length)) 
        //     end = hit.point;

        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
    }
}
