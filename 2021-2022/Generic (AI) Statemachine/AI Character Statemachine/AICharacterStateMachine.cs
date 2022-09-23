using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SensorToolkit;
using FMODUnity;

public class AICharacterStateMachine : AIStateManager, IQuestUpdater
{

    #region AIStates
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    protected AIBaseState<AICharacterStateMachine> currentstate = default;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    public AIIdleState idleState = new AIIdleState();
    public AIChargeState chargeState = new AIChargeState();
    public AIAttackState attackState = new AIAttackState();
    public AIDodgeState dodgeState = new AIDodgeState();
    public AITrippingState trippingState = new AITrippingState();
    public AIRetreatState retreatState = new AIRetreatState();
    public AIDeathState deathState = new AIDeathState();
    #endregion
    public AITypeObject aiType;

    public ParticleSystem weaponParticle; // remove this later for proper weapon system.

    public TriggerSensor sensorTrigger = default;

    [SerializeField] public EventReference attackAudio;


    [HideInInspector] public Animator anim = default;
    [HideInInspector] public NavMeshAgent agent = default;
    [HideInInspector] public AINavMeshRootMotion navMeshRootMotion = default;
    [HideInInspector] public bool stopMovementForAI = false;

    [HideInInspector] public Player_Stats playerTarget;
    
    [HideInInspector] public OnAIDeath onAIDeath;
    [HideInInspector] public OnGainExperience onGainExperience;

    public bool enableWaveMode = false;

    public GameObject thisObject { get => this.gameObject; }
    public IQuestUpdater.OnUpdateQuest onUpdateQuest { get; set; }

    // Start is called before the first frame update
    protected override void Start()
    {
        //Assigning components


        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        navMeshRootMotion = GetComponent<AINavMeshRootMotion>();

        //assigning player location.
        playerTarget = FindObjectOfType<Player_Stats>();

        //State Handeling
        if (!enableWaveMode)
        {
            currentstate = idleState;
            currentstate.EnterState(this);
        }
        else
        {
            currentstate = chargeState;
            currentstate.EnterState(this);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (isAlive)
        {
            currentstate.UpdateState(this);
        }
        else if (currentstate != deathState)
        {
            SwitchState(deathState);
        }

        anim.SetFloat("speed", agent.velocity.magnitude / 3.5f);
    }

    protected void OnDisable()
    {
        currentstate.OnDestroy(this);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        currentstate.OnTriggerEnter(this, other);
    }

    /// <summary>
    /// Switches the current state to the new state.
    /// </summary>
    /// <param name="state"></param>
    public void SwitchState(AIBaseState<AICharacterStateMachine> state)
    {
        currentstate.ExitState(this);
        currentstate = state;
        currentstate.EnterState(this);
    }

    /// <summary>
    /// Sets the movement animation based on speed of the AI.
    /// </summary>
    /// <param name="speed"></param>
    public void SetMovementSpeed(float speed)
    {
        agent.speed = speed;
        anim.SetFloat("speed", speed / 2.5f);
    }

    public void InvokeStumble()
    {
        SwitchState(trippingState);
    }

    public delegate void OnAIDeath();
    public delegate void OnGainExperience(int experience);
}

public enum AI_Type
{
    Melee,
    Range
}