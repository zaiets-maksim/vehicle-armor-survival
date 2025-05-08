using UnityEngine;


public class BulletEventHandler : MonoBehaviour
{
    [SerializeField] private Bullet _bullet; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            Debug.Log("BulletEventHandler");
            Debug.Log("Trigger");
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            enemy.TakeDamage(_bullet, contactPoint);
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            Debug.Log("BulletEventHandler");
            Debug.Log("Collision");
            Vector3 contactPoint = other.collider.ClosestPoint(transform.position);
            enemy.TakeDamage(_bullet, contactPoint);
        }
    }
}