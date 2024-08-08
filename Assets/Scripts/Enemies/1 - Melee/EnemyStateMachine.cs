using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    [Header("States")]
    [HideInInspector] public EnemyIdleState idleState;
    [HideInInspector] public EnemyChaseState chaseState;
    [HideInInspector] public EnemyAttackState attackState;
    [HideInInspector] public EnemyDamageState damageState;
    [HideInInspector] public EnemyDeadState deadState;

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
    
    [Header("Attributes")]
    [Range(0f, 50f)] public float rangeOfView;
    [Range(0f, 25f)] public float rangeOfAttack;
    [Range(0f, 10f)] public float movementSpeed;

    [Header("Damage")]
    public float knockbackDuration;
    [HideInInspector] public Vector3 knockbackVector;
    public bool beingPushed;
    
    // public float damage;
    public float attackCooldownTimer;
    public float attackDuration;
    public float invencibilityTime;


    // [Header("Attack")]
    // public string typeOfAttack;

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

        idleState = new EnemyIdleState(this);
        chaseState = new EnemyChaseState(this);
        attackState = new EnemyAttackState(this);
        damageState = new EnemyDamageState(this);
        deadState = new EnemyDeadState(this);

        canAttack = true;
        canMove = true;
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

