using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SensorToolkit;

public class AIStateManager : MonoBehaviour
{
    protected AIBaseState<AIStateManager> currentstate = default;

    public bool isAlive = true;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentstate.EnterState(this);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentstate.UpdateState(this);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        currentstate.OnTriggerEnter(this, other);
    }

    /*
    /// <summary>
    /// Switches the current state to the new state.
    /// </summary>
    /// <param name="state"></param>
    public virtual void SwitchState(AIBaseState<AIStateManager> state)
    {
        currentstate.ExitState(this);
        currentstate = state;
        currentstate.EnterState(this);
    }
    */
}
