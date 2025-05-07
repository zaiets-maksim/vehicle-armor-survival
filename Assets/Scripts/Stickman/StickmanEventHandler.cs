using System;
using UnityEngine;


public class StickmanEventHandler : MonoBehaviour
{
    [SerializeField] private Stickman _stickman;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bullet bullet))
        {
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            _stickman.TakeDamage(bullet, contactPoint);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Car car))
        {
            _stickman.Death();
        }
    }
}