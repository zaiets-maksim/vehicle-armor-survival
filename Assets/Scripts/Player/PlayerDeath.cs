using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explostion;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _material;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Player _player;

    private void Start()
    {
        // Active();
    }

    public async Task Active()
    {
        _player.Disable();
        _meshRenderer.material = _material;
        _meshRenderer.material.DOColor(new Color(0.14f, 0.14f, 0.14f), 0.2f);
        _explostion.Play();
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.mass = 1;
        _rigidbody.useGravity = true;
        _rigidbody.AddForce(Vector3.up * 7f, ForceMode.Impulse);
        _rigidbody.AddTorque(Vector3.right * Random.Range(-1, 1) * 50f);
        _rigidbody.AddTorque(Vector3.up * Random.Range(-1, 1) * 50f);

        await UniTask.Delay(4000);
    }
}
