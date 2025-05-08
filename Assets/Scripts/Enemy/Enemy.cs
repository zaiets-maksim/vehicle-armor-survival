using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int _health;
    [SerializeField] protected int _damage;

    public int Health => _health;
    public int Damage => _damage;
    
    public void Initialize(EnemyConfig config)
    {
        _health = config.Heath;
        _damage = config.Damage;
    }

    public virtual void TakeDamage(Bullet bullet, Vector3 contactPoint) { }

    public virtual void Death() { }
}
