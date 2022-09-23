using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIBaseState<AICharacterStateMachine>
{
    public override void EnterState(AICharacterStateMachine ai)
    {
        ai.stopMovementForAI = true;
        ai.agent.isStopped = ai.stopMovementForAI;
        ai.SetMovementSpeed(0f);
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
        // If the AI detects the player, and the player is not hidden, then charge at the player.
        if(ai.sensorTrigger.IsDetected(ai.playerTarget.gameObject) && !ai.playerTarget.isHidden)
        {
            ai.SwitchState(ai.chargeState);
        }
    }
}
