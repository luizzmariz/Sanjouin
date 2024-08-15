using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyStateMachine : StateMachine
{

    [Header("Components")]
    [HideInInspector] public PathRequestManager pathRequestManager;
    [HideInInspector] public GameObject playerGameObject;
    [HideInInspector] public Rigidbody2D rigidBody;
    [HideInInspector] public Animator animator;
    [HideInInspector] public EnemyDamageable enemyDamageable;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public CharacterOrientation characterOrientation;
    [HideInInspector] public Animator attackAnimator;
    [HideInInspector] public WaveSpawner waveSpawner;

    [Header("SpawnedInWave")]
    [HideInInspector] public bool spawnedInWave;

    [Header("Bool variables")]
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
    
    [Header("Attack")]
    public float attackCooldownTimer;

    protected virtual void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = transform.GetChild(1).GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        characterOrientation = GetComponent<CharacterOrientation>();
        enemyDamageable = GetComponent<EnemyDamageable>();
        attackAnimator = transform.GetChild(1).GetComponent<Animator>();

        playerGameObject = GameObject.Find("Player");
        pathRequestManager = GameObject.Find("PathfindingManager").GetComponent<PathRequestManager>();
        waveSpawner = GameObject.Find("WaveSpawner").GetComponent<WaveSpawner>();

        canAttack = true;
        canMove = true;
    }

    public virtual IEnumerator Cooldown(string ability)
    {
        yield return null;
    }

    public virtual void TakeDamage(Vector3 knockbackVector) 
    {
        
    }
}

