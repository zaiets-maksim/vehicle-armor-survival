using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StickmanBaseState
{
    public abstract void Enter();
    public abstract void Exit();
    public virtual void Update() { }
}
