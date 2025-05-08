using System;
using UnityEngine;
using Zenject;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Finish finish))
        {
            _player.TryWin();
        }
    }
}
