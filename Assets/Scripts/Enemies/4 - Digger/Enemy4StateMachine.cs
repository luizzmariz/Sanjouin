using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4StateMachine : BaseEnemyStateMachine
{   
    [Header("States")]
    [HideInInspector] public Enemy4IdleState idleState;
    [HideInInspector] public Enemy4ChaseState chaseState;
    [HideInInspector] public Enemy4AttackState attackState;
    [HideInInspector] public Enemy4DigState digState; 
    [HideInInspector] public Enemy4FleeState fleeState;
    [HideInInspector] public Enemy4DamageState damageState;
    [HideInInspector] public Enemy4DeadState deadState;
    
    [Header("Bool variables")]
    public bool canDig;

    [Header("Dig")]
    [Range(0f, 25f)] public float digSpeed;
    public float diggingTime;
    public float maxDigDuration;
    public float digCooldownTimer;

    [Header("Attributes")]
    [Range(0f, 25f)] public float rangeOfDig;

    [Header("Flee")]
    public float fleeDistance;

    protected override void Awake() {
        base.Awake();

        idleState = new Enemy4IdleState(this);
        chaseState = new Enemy4ChaseState(this);
        attackState = new Enemy4AttackState(this);
        digState = new Enemy4DigState(this);
        fleeState = new Enemy4FleeState(this);
        damageState = new Enemy4DamageState(this);
        deadState = new Enemy4DeadState(this);

        canDig = true;
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

            case "dig":
                yield return new WaitForSeconds(digCooldownTimer);
                canDig = true;
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
        Gizmos.DrawWireSphere(transform.position, rangeOfDig);
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