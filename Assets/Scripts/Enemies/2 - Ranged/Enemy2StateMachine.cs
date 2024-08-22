using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy2StateMachine : BaseEnemyStateMachine
{
    [Header("States")]
    [HideInInspector] public Enemy2IdleState idleState;
    [HideInInspector] public Enemy2ChaseState chaseState;
    [HideInInspector] public Enemy2FleeState fleeState;
    [HideInInspector] public Enemy2AttackState attackState;
    [HideInInspector] public Enemy2DamageState damageState;
    [HideInInspector] public Enemy2DeadState deadState;

    [Header("Components")]

    [Header("Bool variables")]
    public bool canFlee;
    public bool isFleeing;
    
    [Header("Attributes")]
    [Range(0f, 25f)] public float rangeOfDanger;

    [Header("Flee")]
    public float fleeCooldownTimer;
    public float fleeDistance;

    [Header("Attack")]
    public float fireForce;
    public GameObject attackProjectile;
    public float attackDuration;

    protected override void Awake() {
        base.Awake();

        idleState = new Enemy2IdleState(this);
        chaseState = new Enemy2ChaseState(this);
        fleeState = new Enemy2FleeState(this);
        attackState = new Enemy2AttackState(this);
        damageState = new Enemy2DamageState(this);
        deadState = new Enemy2DeadState(this);

        canFlee = true;
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
        Gizmos.DrawWireSphere(transform.position, rangeOfDanger);
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

