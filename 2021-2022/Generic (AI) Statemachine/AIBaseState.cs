using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBaseState<T> where T : AIStateManager
{
    public abstract void EnterState(T ai);

    public abstract void UpdateState(T ai);

    public abstract void OnTriggerEnter(T ai, Collider other);

    public abstract void OnDestroy(T ai);

    public abstract void ExitState(T ai);
}
