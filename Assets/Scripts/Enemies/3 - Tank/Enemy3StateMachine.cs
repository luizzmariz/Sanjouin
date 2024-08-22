using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3StateMachine : BaseEnemyStateMachine
{   
    [Header("States")]
    [HideInInspector] public Enemy3IdleState idleState;
    [HideInInspector] public Enemy3ChaseState chaseState;
    [HideInInspector] public Enemy3AttackState attackState;
    [HideInInspector] public Enemy3RunState runState; 
    [HideInInspector] public Enemy3DamageState damageState;
    [HideInInspector] public Enemy3DeadState deadState;
    
    [Header("Bool variables")]
    public bool canRun;

    [Header("Run")]
    public float chargingRunTimer;
    public float runCooldownTimer;
    public float maxRunDuration;
    public Collider2D runCollider;
    public float runDamage;

    [Header("Attributes")]
    [Range(0f, 25f)] public float rangeOfEngage;
    [Range(0f, 25f)] public float runSpeed;

    protected override void Awake() {
        base.Awake();

        idleState = new Enemy3IdleState(this);
        chaseState = new Enemy3ChaseState(this);
        attackState = new Enemy3AttackState(this);
        runState = new Enemy3RunState(this);
        damageState = new Enemy3DamageState(this);
        deadState = new Enemy3DeadState(this);

        canRun = true;
    }

    protected override BaseState GetInitialState() {
        return idleState;
    }

    public override IEnumerator Cooldown(string ability)
    {
        switch(ability)
        {
            case "attack":
                // Debug.Log("attack cooldown started");
                yield return new WaitForSeconds(attackCooldownTimer);
                // Debug.Log("attack cooldown ended");
                canAttack = true;
            break;

            case "run":
                yield return new WaitForSeconds(runCooldownTimer);
                canRun = true;
            break;

            default:
            break;
        }
    }

    public IEnumerator LoadRun(string ability)
    {
        switch(ability)
        {
            case "run":
                yield return new WaitForSeconds(chargingRunTimer);
                ChangeState(runState);
            break;

            default:
            break;
        }
    }

    // private void OnGUI()
    // {
    //     GUILayout.BeginArea(new Rect(250, 125, 200f, 150f));
    //     string content = currentState != null ? currentState.name : "(no current state)";
    //     GUILayout.Label($"<color='red'><size=40>{content}</size></color>");
    //     GUILayout.EndArea();
    // }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeOfView);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeOfAttack);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeOfEngage);
    }

    public override void TakeDamage(Vector3 knockbackVector) 
    {
        if(enemyDamageable.currentHealth <= 0)
        {
            ChangeState(deadState);
        }
        else
        {
            this.knockbackVector = knockbackVector;
            ChangeState(damageState);
        }
    }
}