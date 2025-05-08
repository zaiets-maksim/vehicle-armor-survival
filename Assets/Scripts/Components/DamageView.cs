using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using tetris.Scripts.Extensions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DamageView : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Text _text;
    
    private Transform _cameraTransform;
    private Coroutine _faceCameraCoroutine;
    private IGameFactory _gameFactory;

    [Inject]
    public void Constructor(IGameFactory gameFactory)
    {
        _gameFactory = gameFactory;
    }
    
    private void Start()
    {
        _cameraTransform = _gameFactory.Camera.transform;
    }

    public async void ShowDamage(int damage)
    {
        _text.text = damage.ToString();
        _canvasGroup.alpha = 1f;
        
        if(_faceCameraCoroutine != null)
            StopCoroutine(_faceCameraCoroutine);
        _faceCameraCoroutine = StartCoroutine(FaceCamera(1.5f));
        
        await UniTask.Delay(1.5f.ToMiliseconds());
        _canvasGroup.DOFade(0f, 0.3f);

    }
    
    private IEnumerator FaceCamera(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.LookAt(transform.position + _cameraTransform.forward, Vector3.up);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
