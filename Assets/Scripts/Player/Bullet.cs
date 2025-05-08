using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _lifetime;
    public int Damage => _damage;

    public IEnumerator Move(Vector3 velocity)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _lifetime)
        {
            transform.position += (velocity + transform.forward * _speed) * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        Destroy(gameObject);
    }

    public void AlightByHeight(float height) => transform.DOMoveY(height, 0.1f);
}
