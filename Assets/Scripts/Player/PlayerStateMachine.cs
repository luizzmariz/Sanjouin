using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : StateMachine
{
    //States
    [HideInInspector] public PlayerIdleState idleState;
    [HideInInspector] public PlayerMoveState moveState;
    [HideInInspector] public PlayerAttackState attackState;
    [HideInInspector] public PlayerDashState dashState;
    [HideInInspector] public PlayerDamageState damageState;
    [HideInInspector] public PlayerDeadState deadState;
    
    // [HideInInspector] public DeadState deadState;

    //Global information
    
    //GameObject information
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public Rigidbody2D rigidBody;
    // public Animator animator;
    // public SpriteRenderer spriteRenderer;
    [HideInInspector] public CharacterOrientation characterOrientation;
    [HideInInspector] public PlayerDamageable playerDamageable;
    // public WeaponManager weaponManager;
    // public TrailRenderer trailRenderer;

    [Header("Bool variables")]
    public bool canMove;
    public bool canDash;
    public bool canAttack;

    [Header("Movement")]
    public float runningMultiplier;
    public float movementSpeed;

    [Header("Dash")]
    public bool isDashing;
    public float dashingPower;
    public float dashCooldownTime;
    public float dashingTime;

    [Header("Damage")]
    public float knockbackDuration;
    [HideInInspector] public Vector3 knockbackVector;
    public bool beingPushed;

    [Header("Attack")]
    public bool isAttacking;
    public int attackType;
    public float attackDuration;
    public float attack1CooldownTimer;
    public float attack2CooldownTimer;

    [Header("InvencibilityTime")]
    public float invencibilityTime;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        // playerInput.actions.FindActionMap("UI").Enable();

        rigidBody = GetComponent<Rigidbody2D>();
        // animator = GetComponent<Animator>();
        // spriteRenderer = GetComponent<SpriteRenderer>();
        characterOrientation = GetComponent<CharacterOrientation>();
        // weaponManager = GetComponentInChildren<WeaponManager>();
        playerDamageable = GetComponent<PlayerDamageable>();
        // trailRenderer = GetComponentInChildren<TrailRenderer>();

        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);
        attackState = new PlayerAttackState(this);
        dashState = new PlayerDashState(this);
        damageState = new PlayerDamageState(this);
        deadState = new PlayerDeadState(this);
    }

    protected override BaseState GetInitialState() {
        return idleState;
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(450, 125, 200f, 150f));
        string content = currentState != null ? currentState.name : "(no current state)";
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
        GUILayout.EndArea();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(canMove)
            {
                ChangeState(moveState);
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(canAttack)
            {
                attackType = 1;
                ChangeState(attackState);
            }
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(canAttack)
            {
                attackType = 2;
                ChangeState(attackState);
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(canDash && !isDashing)
            {
                ChangeState(dashState);
            }
        }
    }

    public IEnumerator Cooldown(string ability)
    {
        switch(ability)
        {
            case "dash":
            yield return dashCooldownTime;
            canDash = true;
            break;

            default:
            break;
        }
        
    }

    //we need to change this after we got some basic character animations - this functions need to be called by the character animations and not by the attack animation
    public void CastAttackEnded()
    {
        canAttack = true;
        isAttacking = false;
    }

    public void DashEnded()
    {
        isDashing = false;
    }

    // void OnDisable()
    // {
    //     playerInput.actions.FindActionMap("Player").Disable();
    // }

    // void OnEnable()
    // {
    //     playerInput.actions.FindActionMap("Player").Enable();
    // }
}