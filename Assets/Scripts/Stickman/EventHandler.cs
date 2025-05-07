using Logic.Collisions;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    [SerializeField] private TriggerObserver _triggerObserver;

    private void OnValidate()
    {
        if (!_triggerObserver) TryGetComponent(out _triggerObserver);
    }

    protected virtual void Initialize()
    {
    }

    protected virtual void OnEnable()
    {
        _triggerObserver.TriggerEnter += ValidateEnter;
        _triggerObserver.TriggerStay += ValidateStay;
        _triggerObserver.TriggerExit += ValidateExit;
    }

    protected virtual void OnDisable()
    {
        _triggerObserver.TriggerEnter -= ValidateEnter;
        _triggerObserver.TriggerStay -= ValidateStay;
        _triggerObserver.TriggerExit -= ValidateExit;
    }

    protected virtual void ValidateEnter(Collider collider)
    {
    }

    protected virtual void ValidateStay(Collider collider)
    {
    }

    protected virtual void ValidateExit(Collider collider)
    {
    }
}