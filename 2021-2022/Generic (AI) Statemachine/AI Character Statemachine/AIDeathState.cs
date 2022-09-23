using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIBaseState<AICharacterStateMachine>
{
    public override void EnterState(AICharacterStateMachine ai)
    {
        ai.onAIDeath += Death;
        ai.onAIDeath();

        if (ai.onUpdateQuest != null)
        {
            ai.onUpdateQuest();
        }

        ai.onGainExperience += NielsDev.Leveling.LevelManager.Instance.AddExperience;
        ai.onGainExperience(ai.aiType.experienceGainOnDeath);
    }

    public override void ExitState(AICharacterStateMachine ai)
    {

    }

    public override void OnTriggerEnter(AICharacterStateMachine ai, Collider other)
    {

    }

    public override void UpdateState(AICharacterStateMachine ai)
    {

    }

    public void Death()
    {

    }

    public override void OnDestroy(AICharacterStateMachine ai)
    {

    }
}
