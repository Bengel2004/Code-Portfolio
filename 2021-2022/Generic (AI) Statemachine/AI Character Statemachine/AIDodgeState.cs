using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDodgeState : AIBaseState<AICharacterStateMachine>
{
    public override void EnterState(AICharacterStateMachine ai)
    {
        float angle = 70f; // only if the player is facing the enemy will they dodge back.
        if (Vector3.Angle(ai.playerTarget.transform.forward, ai.transform.position - ai.playerTarget.transform.position) < angle)
        {
            ai.navMeshRootMotion.InvokeDodgeBack();
        }
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

    private IEnumerator StartDodge(AICharacterStateMachine ai)
    {
        yield return new WaitForSeconds(Random.Range(0, 2f));
        ai.navMeshRootMotion.InvokeDodgeBack();
    }
}
