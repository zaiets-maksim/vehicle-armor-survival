using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Vector3 _defaultPosition;
    private bool _isActive;

    private void Start()
    {
        _defaultPosition = transform.position;
    }

    private void Update()
    {
        if(!_isActive)
            return;
        
        var pos = _defaultPosition + _target.position;
        pos.x = _target.position.x / 5f;
        transform.position = pos;
    }

    public void Initialize(Player player)
    {
        _target = player.transform;
        _isActive = true;
    }
}
