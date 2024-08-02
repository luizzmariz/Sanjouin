using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    //States
    [HideInInspector] public EnemyIdleState idleState;
    [HideInInspector] public EnemyChaseState chaseState;
    // [HideInInspector] public HitState hitState;
    [HideInInspector] public EnemyAttackState attackState;

    // //Global information
    [HideInInspector] public PathRequestManager pathRequestManager;
    [HideInInspector] public GameObject playerGameObject;
    
    //GameObject information
    [Header("Holder Components")]
    [HideInInspector] public Rigidbody2D rigidBody;
    // public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    // public CharacterOrientation characterOrientation;
    // public EnemyDamageable enemyDamageable;
    // public Animator attackAnimator;

    [Header("Bool variables")]
    public bool canMove;
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
        rigidBody = GetComponent<Rigidbody2D>();
        // animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        // characterOrientation = GetComponent<CharacterOrientation>();
        // enemyDamageable = GetComponent<EnemyDamageable>();

        playerGameObject = GameObject.Find("Player");
        pathRequestManager = GameObject.Find("PathfindingManager").GetComponent<PathRequestManager>();

        idleState = new EnemyIdleState(this);
        chaseState = new EnemyChaseState(this);
        // hitState = new HitState(this);
        attackState = new EnemyAttackState(this);
    }


    protected override BaseState GetInitialState() {
        return idleState;
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(250, 125, 200f, 150f));
        string content = currentState != null ? currentState.name : "(no current state)";
        GUILayout.Label($"<color='red'><size=40>{content}</size></color>");
        GUILayout.EndArea();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeOfView);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeOfAttack);
    }

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

