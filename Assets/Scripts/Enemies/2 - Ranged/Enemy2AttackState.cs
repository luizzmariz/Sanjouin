using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2AttackState : BaseState
{
    Enemy2StateMachine enemyStateMachine;

    Vector3 holderPosition;
    Vector3 playerPosition;

    bool hasAttacked;

    public Enemy2AttackState(Enemy2StateMachine stateMachine) : base("Attacking", stateMachine)
    {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() 
    {
        enemyStateMachine.rigidBody.velocity = Vector3.zero;

        enemyStateMachine.canMove = false;
        enemyStateMachine.canAttack = false;
        enemyStateMachine.enemyDamageable.damageable = true;
        enemyStateMachine.isAttacking = true;
        hasAttacked = false;
    }

    public override void UpdateLogic() {
        if(!enemyStateMachine.isAttacking)
        {
            hasAttacked = false;
            // Debug.Log("!enemyStateMachine.isAttacking");
            enemyStateMachine.StartCoroutine(enemyStateMachine.Cooldown("attack"));

            holderPosition = enemyStateMachine.transform.position;
            playerPosition = enemyStateMachine.playerGameObject.transform.position;

            if(Vector3.Distance(holderPosition, playerPosition) > enemyStateMachine.rangeOfAttack)
            {
                if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfView)
                {
                    stateMachine.ChangeState(enemyStateMachine.chaseState);
                }
                else
                {
                    stateMachine.ChangeState(enemyStateMachine.idleState);
                }
            }
            else
            {
                stateMachine.ChangeState(enemyStateMachine.idleState);
            }
        }
    }

    public override void UpdatePhysics() {
        if(!hasAttacked)
        {
            Attack();
        }
    }

    public void Attack()
    {
        // Debug.Log("Attack");
        enemyStateMachine.attackAnimator.SetTrigger("Attack");
        hasAttacked = true;
    }

    public void AttackEnded()
    {
        // enemyStateMachine.animator.SetTrigger("attackEnd");
        stateMachine.ChangeState(enemyStateMachine.idleState);
        enemyStateMachine.attackCooldownTimer = enemyStateMachine.attackDuration;
    }
}
