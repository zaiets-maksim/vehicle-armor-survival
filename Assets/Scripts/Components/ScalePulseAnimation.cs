using DG.Tweening;
using UnityEngine;

public class ScalePulseAnimation : MonoBehaviour
{
    public float zoomAmount = 1.2f;
    public float zoomDuration = 0.5f;
    private void Start()
    {
        StartAnimation();
    }

    public void StartAnimation()
    {
        Sequence zoomSequence = DOTween.Sequence();
        zoomSequence.Append(transform.DOScale(Vector3.one * zoomAmount, zoomDuration).SetEase(Ease.InOutSine));
        zoomSequence.Append(transform.DOScale(Vector3.one, zoomDuration).SetEase(Ease.InOutSine));
        zoomSequence.SetLoops(-1, LoopType.Yoyo);
    }
}
