using System.Collections;
using UnityEngine;

public class BaseCreatureStateMachine : StateMachine
{

    [Header("Components")]
    [HideInInspector] public PathRequestManager pathRequestManager;
    [HideInInspector] public GameObject playerGameObject;
    [HideInInspector] public Rigidbody2D rigidBody;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public CharacterOrientation characterOrientation;
    [HideInInspector] public CreatureSpawner creatureSpawner;
    [HideInInspector] public SpriteRenderer bodySpriteRenderer;
    [HideInInspector] public SpriteRenderer handsSpriteRenderer;

    [Header("SpawnedInWave")]
    [HideInInspector] public bool spawnedByRegularLogic;

    [Header("Bool variables")]
    public bool canMove;
    public bool canAttack;
    public bool isAttacking;

    public bool stopped = false;
    
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
        animator = transform.Find("Visual").GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        characterOrientation = GetComponent<CharacterOrientation>();
        bodySpriteRenderer = transform.Find("Visual").GetComponent<SpriteRenderer>();
        handsSpriteRenderer= transform.Find("Hands").GetComponent<SpriteRenderer>();

        playerGameObject = GameObject.Find("Player");
        pathRequestManager = GameObject.Find("PathfindingManager").GetComponent<PathRequestManager>();
        creatureSpawner = GameObject.Find("CreatureSpawner").GetComponent<CreatureSpawner>();

        canAttack = true;
        canMove = true;
    }

    public virtual IEnumerator Cooldown(string ability)
    {
        yield return null;
    }
}

