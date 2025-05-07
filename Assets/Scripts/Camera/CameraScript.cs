using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Vector3 _defaultPosition;

    private void Start()
    {
        _defaultPosition = transform.position;
    }

    private void Update()
    {
        var pos = _defaultPosition + _target.position;
        pos.x = _target.position.x / 5f;
        transform.position = pos;
    }
}
