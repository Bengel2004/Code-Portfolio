using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIChargeState : AIBaseState<AICharacterStateMachine>
{
    private float chargeSpeed = 2.5f;

    private float timeToLoseSpottedSight = 15f;
    private float spottingTimeStamp = 0.0f;
    private bool targetSpotted = false;



    public override void EnterState(AICharacterStateMachine ai)
    {
        // Allow the AI to move when charging!
        ai.stopMovementForAI = false;
        ai.agent.isStopped = ai.stopMovementForAI;

        ai.SetMovementSpeed(chargeSpeed);
    }

    public override void ExitState(AICharacterStateMachine ai)
    {

    }

    public override void OnTriggerEnter(AICharacterStateMachine ai, Collider other)
    {

    }

    public override void UpdateState(AICharacterStateMachine ai)
    {
        bool playerIsInRange = Vector3.Distance(ai.playerTarget.transform.position, ai.transform.position) < ai.aiType.engagementDistance;
        bool isGoingToDodgePlayerAttack = Vector3.Distance(ai.playerTarget.transform.position, ai.transform.position) < 10f && Vector3.Angle(ai.playerTarget.transform.forward, ai.transform.position - ai.playerTarget.transform.position) < 90f;

        if (ai.playerTarget.isHidden)
        {
            // If the player decides to hide suddenly, then the AI will lose him.
            ai.SwitchState(ai.idleState);
        }

        CheckIfTargetSpotted(ai);

        if (!ai.enableWaveMode)
        {
            if (targetSpotted)
            {
                if (playerIsInRange)
                {
                    // If the AI is in range of the Tree, then Open Fire
                    ai.SwitchState(ai.attackState);
                }
                else if (IsFireButtonPressed() && isGoingToDodgePlayerAttack)
                {
                    ai.SwitchState(ai.dodgeState);
                }
                else if (!playerIsInRange)
                {
                    // If the AI is not in range, charge at it.
                    ai.agent.SetDestination(ai.playerTarget.transform.position);
                }
            }
            else if (!targetSpotted)
            {
                // If the AI is not seen by the trigger, go back to idle.
                ai.SwitchState(ai.idleState);
            }
        }
        else // This allows the AI to just continue to run and chase the player sorta like a "Wave fighting system"
        {
            if (playerIsInRange)
            {
                // If the AI is in range of the Tree, then Open Fire
                ai.SwitchState(ai.attackState);
            }
            else if (IsFireButtonPressed() && isGoingToDodgePlayerAttack)
            {
                ai.SwitchState(ai.dodgeState);
            }
            else if (!playerIsInRange)
            {
                // If the AI is not in range, charge at it.
                ai.agent.SetDestination(ai.playerTarget.transform.position);
            }
        }




    }

    /// <summary>
    /// Checks if the player is either musing a mouse or a joystick
    /// </summary>
    /// <returns></returns>
    private bool IsFireButtonPressed()
    {
        if(Gamepad.current != null)
        {
            return Gamepad.current.rightTrigger.wasPressedThisFrame ? true : Mouse.current.leftButton.wasPressedThisFrame;
        }
        else
        {
            return Mouse.current.leftButton.wasPressedThisFrame;
        }
    }

    public void CheckIfTargetSpotted(AICharacterStateMachine ai)
    {
        if(ai.sensorTrigger.IsDetected(ai.playerTarget.gameObject) || spottingTimeStamp > Time.time)
            targetSpotted = true;
        else
            targetSpotted = false;

        if (ai.sensorTrigger.IsDetected(ai.playerTarget.gameObject))
            spottingTimeStamp = Time.time + timeToLoseSpottedSight;
    }

    public override void OnDestroy(AICharacterStateMachine ai)
    {

    }
}
