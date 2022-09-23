using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITrippingState : AIBaseState<AICharacterStateMachine>
{
    public override void EnterState(AICharacterStateMachine ai)
    {
        ai.navMeshRootMotion.InvokeStumbleBack();
    }

    public override void ExitState(AICharacterStateMachine ai)
    {

    }

    public override void OnDestroy(AICharacterStateMachine ai)
    {

    }

    public override void OnTriggerEnter(AICharacterStateMachine ai, Collider other)
    {

    }

    public override void UpdateState(AICharacterStateMachine ai)
    {
        if (ai.sensorTrigger.IsDetected(ai.playerTarget.gameObject) && Vector3.Distance(ai.playerTarget.transform.position, ai.transform.position) > ai.aiType.engagementDistance)
        {
            ai.SwitchState(ai.chargeState);
        }
        else if (ai.sensorTrigger.IsDetected(ai.playerTarget.gameObject) && Vector3.Distance(ai.playerTarget.transform.position, ai.transform.position) < ai.aiType.engagementDistance)
        {
            ai.SwitchState(ai.attackState);
        }
    }
}
