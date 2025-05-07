using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Extensions;
using UnityEngine;

public class StickmanStateMachine : MonoBehaviour
{
    protected List<StickmanBaseState> _states;
    protected StickmanBaseState _currentState;

    private bool _isTransitioning;

    public bool IsTransitioning => _isTransitioning;
    public StickmanBaseState CurrentState => _currentState;

    protected List<StickmanBaseState> CreateStates(params StickmanBaseState[] states) =>
        new(states);

    public async void ChangeState<T>() where T : StickmanBaseState
    {
        if (_isTransitioning)
        {
            Debug.LogWarning($"Blocked state change to {typeof(T).Name} — transition in progress ({gameObject.GetInstanceID()})");
            return;
        }

        var state = _states.FirstOrDefault(s => s is T);
        if (state == _currentState)
            return;

        _isTransitioning = true;

        Debug.Log(Make.Colored($"Going to {state.GetType().Name} {gameObject.GetInstanceID()}", Color.yellow));

        _currentState?.Exit();

        await UniTask.Yield();

        state?.Enter();
        _currentState = state;

        Debug.Log(Make.Colored($"-> {_currentState.GetType().Name} {gameObject.GetInstanceID()}", Color.green));

        _isTransitioning = false;
    }
}