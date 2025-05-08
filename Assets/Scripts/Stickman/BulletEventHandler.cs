using UnityEngine;


public class BulletEventHandler : MonoBehaviour
{
    [SerializeField] private Bullet _bullet; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            enemy.TakeDamage(_bullet, contactPoint);
        }
    }
}