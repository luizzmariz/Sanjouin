using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    //States
    [HideInInspector] public EnemyIdleState idleState;
    // [HideInInspector] public ChaseState chaseState;
    // [HideInInspector] public HitState hitState;
    [HideInInspector] public EnemyAttackState attackState;

    // //Global information
    // [HideInInspector] public PathRequestManager pathRequestManager;
    [HideInInspector] public GameObject playerGameObject;
    
    //GameObject information
    [Header("Holder Components")]
    public Rigidbody rigidBody;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    // public CharacterOrientation characterOrientation;
    // public EnemyDamageable enemyDamageable;
    public Animator attackAnimator;

    [Header("Bool variables")]
    public bool canMove;
    public bool canDash;
    public bool canAttack;
    
    [Header("Attributes")]
    [Range(0f, 50f)] public float rangeOfView;
    [Range(0f, 25f)] public float rangeOfAttack;
    [Range(0f, 10f)] public float movementSpeed;
    public float damage;
    public float attackCooldownTimer;
    public float attackDuration;
    public float invencibilityTime;


    // [Header("Attack")]
    // public string typeOfAttack;

    private void Awake() {
        GetInfo();

        idleState = new EnemyIdleState(this);
        // chaseState = new ChaseState(this);
        // hitState = new HitState(this);
        attackState = new EnemyAttackState(this);
    }

    public void GetInfo()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // characterOrientation = GetComponent<CharacterOrientation>();
        // enemyDamageable = GetComponent<EnemyDamageable>();

        // playerGameObject = GameObject.Find("Player");
        // pathRequestManager = GameObject.Find("PathfindingManager").GetComponent<PathRequestManager>();
    }

    // protected override BaseState GetInitialState() {
    //     return idleState;
    // }

    // private void OnGUI()
    // {
    //     GUILayout.BeginArea(new Rect(10f, 10f, 200f, 100f));
    //     string content = currentState != null ? currentState.name : "(no current state)";
    //     GUILayout.Label($"<color='red'><size=40>{content}</size></color>");
    //     GUILayout.EndArea();
    // }

    // public void ChargingAttackSucessfull()
    // {
    //     if(currentState == chargingState)
    //     {
    //         chargingState.Attack();
    //     }
    // }

    // public void CastAttackEnded()
    // {
    //     chargingState.AttackEnded();
    // }
}

