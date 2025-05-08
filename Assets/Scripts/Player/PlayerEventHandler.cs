using UnityEngine;

public class PlayerEventHandler : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            _player.TakeDamage(enemy.Damage);
            enemy.Death();
        }
    }
}
