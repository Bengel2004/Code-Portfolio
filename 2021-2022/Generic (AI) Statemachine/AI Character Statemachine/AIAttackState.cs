using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIAttackState : AIBaseState<AICharacterStateMachine>
{
    private float nextTimeToGetRandomAttack = 0.0f;

    private float nextTimeToFire = 0.0f;
    private float rateOfFire = 60f;

    private FMOD.Studio.EventInstance flameThrowerAudioInstance;
    public override void EnterState(AICharacterStateMachine ai)
    {
        nextTimeToFire = Time.time;

        // stop AI from moving when firing!
        ai.stopMovementForAI = true;
        ai.agent.isStopped = ai.stopMovementForAI;

        //Enable firing animation.
        ai.anim.SetBool("isFiring", true);


        if (ai.aiType.type == AI_Type.Range)
        {
            flameThrowerAudioInstance = FMODUnity.RuntimeManager.CreateInstance(ai.attackAudio);
            flameThrowerAudioInstance.start();
            flameThrowerAudioInstance.release();

            FMODUnity.RuntimeManager.AttachInstanceToGameObject(flameThrowerAudioInstance, ai.transform);
            ai.weaponParticle.Play();
        }
    }
    
    public override void OnTriggerEnter(AICharacterStateMachine ai, Collider other)
    {

    }

    public override void UpdateState(AICharacterStateMachine ai)
    {
        float angle = 40;

        // Is the player still in range? Look at it, if not, switch back to charging it.
        if (ai.sensorTrigger.IsDetected(ai.playerTarget.gameObject) && Vector3.Distance(ai.playerTarget.transform.position, ai.transform.position) > ai.aiType.engagementDistance)
        {
            ai.SwitchState(ai.chargeState);
        }
        //else if(Vector3.Angle(ai.playerTarget.transform.forward, ai.transform.position - ai.playerTarget.transform.position) < angle && Input.GetMouseButtonDown(0))
        else if(IsFireButtonPressed())
        {
            ai.SwitchState(ai.dodgeState);
        }
        else
        {
            Vector3 playerTargetLookAt = ai.playerTarget.transform.position;
            playerTargetLookAt.y = ai.transform.position.y;
            ai.transform.LookAt(playerTargetLookAt);


            if (Time.time > nextTimeToFire)
            {
                nextTimeToGetRandomAttack = Time.time + 1f;
                SelectRandomAttack(ai);
            }
        }
    }

    /// <summary>
    /// Fires on the enemy
    /// </summary>
    public void Fire()
    {
        if (Time.time > nextTimeToFire)
        {
            nextTimeToFire = Time.time + (rateOfFire / 60f);
        }
    }

    public override void ExitState(AICharacterStateMachine ai)
    {
        if (ai.aiType.type == AI_Type.Range)
        {
            flameThrowerAudioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            flameThrowerAudioInstance.release();

            ai.weaponParticle.Stop();
        }
        ai.anim.SetBool("isFiring", false);
    }

    private void SelectRandomAttack(AICharacterStateMachine ai)
    {
        ai.anim.SetInteger("randomAttack", Mathf.RoundToInt(Random.Range(0, 2f)));   
    }

    /// <summary>
    /// Checks if the player is either musing a mouse or a joystick
    /// </summary>
    /// <returns></returns>
    private bool IsFireButtonPressed()
    {
        if (Gamepad.current != null)
        {
            return Gamepad.current.rightTrigger.wasPressedThisFrame ? true : Mouse.current.leftButton.wasPressedThisFrame;
        }
        else
        {
            return Mouse.current.leftButton.wasPressedThisFrame;
        }
    }

    public override void OnDestroy(AICharacterStateMachine ai)
    {
        flameThrowerAudioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        flameThrowerAudioInstance.release();
    }
}
