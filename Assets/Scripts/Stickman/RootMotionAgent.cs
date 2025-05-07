using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
public class RootMotionAgent : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _turnRadius = 2f;
    [SerializeField] private float _minSpeedFactor = 0.2f;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _stoppingDistance = 1f;
    
    private Vector3 _targetPosition;
    private bool _hasDestination;

    public event Action OnDestinationReached;

    public void SetDestination(Vector3 position)
    {
        _targetPosition = position;
        _hasDestination = true;
    }

    private void OnAnimatorMove()
    {
        if (!_hasDestination)
        {
            transform.position += _animator.deltaPosition * _speed;
            transform.forward = _animator.deltaRotation * transform.forward * _speed;
            return;
        }

        Vector3 delta = _animator.deltaPosition * _speed;
        delta.y = 0;

        Vector3 velocity = delta / Time.deltaTime;
        float speed = velocity.magnitude;
        Vector3 toTarget = _targetPosition - transform.position;
        toTarget.y = 0;

        if (toTarget.magnitude < _stoppingDistance)
        {
            OnDestinationReached?.Invoke();
            Debug.Log("OnDestinationReached");
            _hasDestination = false;
            return;
        }

        Vector3 desiredDir = toTarget.normalized;
        Vector3 forward = transform.forward;

        float angle = Vector3.SignedAngle(forward, desiredDir, Vector3.up);
        
        float maxAngle = Mathf.Rad2Deg * (speed / _turnRadius) * Time.deltaTime;
        float clampedAngle = Mathf.Clamp(angle, -maxAngle, maxAngle);

        transform.rotation *= Quaternion.Euler(0, clampedAngle, 0);
        
        float angleFactor = Mathf.InverseLerp(0, 180, Mathf.Abs(angle));
        float speedFactor = Mathf.Lerp(1f, _minSpeedFactor, angleFactor);

        transform.position += forward * speed * speedFactor * Time.deltaTime;
    }
}