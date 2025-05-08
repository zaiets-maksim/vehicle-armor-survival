using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _sidePoint;
    [SerializeField] private Transform _startPoint;

    private IGameCurator _gameCurator;
    
    private Vector3 _defaultPosition;
    private bool _isActive;

    [Inject]
    public void Constructor(IGameCurator gameCurator)
    {
        _gameCurator = gameCurator;
    }

    private void Start()
    {
        _gameCurator.OnStartGame += ToStartPosition;
        _gameCurator.OnEndGame += Disable;
    }

    private void OnDestroy()
    {
        _gameCurator.OnStartGame -= ToStartPosition;
        _gameCurator.OnEndGame -= Disable;
    }

    private void Update()
    {
        if (!_isActive)
            return;

        var pos = _defaultPosition + _target.position;
        pos.x = _target.position.x / 5f;
        transform.position = pos;
    }
    
    public void Enable() => _isActive = true;

    private void Disable(GameResult result) => _isActive = false;

    public void Initialize(Player player)
    {
        _target = player.transform;
        transform.SetPositionAndRotation(_sidePoint.position, _sidePoint.rotation);
    }

    private void ToStartPosition()
    {
        _defaultPosition = _startPoint.position;
        StartCoroutine(ToStartPoint(1f, Enable));
    }

    private IEnumerator ToStartPoint(float duration, Action callback)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            var pos = _defaultPosition + _target.position;
            transform.position = Vector3.Lerp(transform.position, pos, elapsedTime / duration);
            transform.rotation = Quaternion.Lerp(transform.rotation, _startPoint.rotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        callback?.Invoke();
    }
}