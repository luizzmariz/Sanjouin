using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy2StateMachine : StateMachine
{
    [Header("States")]
    [HideInInspector] public Enemy2IdleState idleState;
    [HideInInspector] public Enemy2ChaseState chaseState;
    [HideInInspector] public Enemy2FleeState fleeState;
    [HideInInspector] public Enemy2AttackState attackState;
    [HideInInspector] public Enemy2DamageState damageState;
    [HideInInspector] public Enemy2DeadState deadState;

    [Header("Components")]
    [HideInInspector] public PathRequestManager pathRequestManager;
    [HideInInspector] public GameObject playerGameObject;
    [HideInInspector] public Rigidbody2D rigidBody;
    // public Animator animator;
    [HideInInspector] public EnemyDamageable enemyDamageable;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public CharacterOrientation characterOrientation;
    [HideInInspector] public Animator attackAnimator;
    [HideInInspector] public WaveSpawner waveSpawner;

    [Header("Bool variables")]
    [HideInInspector] public bool spawnedInWave;
    public bool canMove;
    public bool canAttack;
    public bool isAttacking;
    public bool canFlee;
    public bool isFleeing;
    
    [Header("Attributes")]
    [Range(0f, 50f)] public float rangeOfView;
    [Range(0f, 25f)] public float rangeOfAttack;
    [Range(0f, 25f)] public float rangeOfDanger;
    [Range(0f, 10f)] public float movementSpeed;

    [Header("Damage")]
    public float knockbackDuration;
    [HideInInspector] public Vector3 knockbackVector;
    public bool beingPushed;
    
    [Header("Attack")]
    public float attackCooldownTimer;
    public float attackDuration;

    [Header("Flee")]
    public float fleeCooldownTimer;
    public float fleeDistance;

    private void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        // animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        characterOrientation = GetComponent<CharacterOrientation>();
        enemyDamageable = GetComponent<EnemyDamageable>();
        attackAnimator = transform.GetChild(1).GetComponent<Animator>();

        playerGameObject = GameObject.Find("Player");
        pathRequestManager = GameObject.Find("PathfindingManager").GetComponent<PathRequestManager>();
        waveSpawner = GameObject.Find("WaveSpawner").GetComponent<WaveSpawner>();

        idleState = new Enemy2IdleState(this);
        chaseState = new Enemy2ChaseState(this);
        fleeState = new Enemy2FleeState(this);
        attackState = new Enemy2AttackState(this);
        damageState = new Enemy2DamageState(this);
        deadState = new Enemy2DeadState(this);

        canAttack = true;
        canMove = true;
        canFlee = true;
    }

    protected override BaseState GetInitialState() {
        return idleState;
    }

    public IEnumerator Cooldown(string ability)
    {
        switch(ability)
        {
            case "attack":
            // Debug.Log("attack cooldown started");
            yield return new WaitForSeconds(attackCooldownTimer);
            // Debug.Log("attack cooldown ended");
            canAttack = true;
            break;

            case "flee":
            // Debug.Log("attack cooldown started");
            yield return new WaitForSeconds(fleeCooldownTimer);
            // Debug.Log("attack cooldown ended");
            canFlee = true;
            break;

            default:
            break;
        }
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeOfDanger);
    }
}

