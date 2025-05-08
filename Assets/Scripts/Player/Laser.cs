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

    public void Enable()
    {
        _lineRenderer.enabled = true;
    }
    
    public void Disable()
    {
        _lineRenderer.enabled = false;
        enabled = false;
    }

    private void Update()
    {
        Vector3 start = _firePoint.position;
        Vector3 end = start + _firePoint.forward * _length;
        end.y = 1.2f;
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
    }
}
