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
            holderPosition = enemyStateMachine.transform.position;
            playerPosition = enemyStateMachine.playerGameObject.transform.position;

            if(Vector3.Distance(holderPosition, playerPosition) < enemyStateMachine.rangeOfDanger && enemyStateMachine.canFlee)
            {
                stateMachine.ChangeState(enemyStateMachine.fleeState);
            }
            else if(Vector3.Distance(holderPosition, playerPosition) > enemyStateMachine.rangeOfAttack)
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
            holderPosition = enemyStateMachine.transform.position;
            playerPosition = enemyStateMachine.playerGameObject.transform.position;

            Vector3 attackDirection = playerPosition - holderPosition;
            enemyStateMachine.enemyHands.Attack(attackDirection);
            hasAttacked = true;
        }
    }

    public override void Exit() 
    {
        enemyStateMachine.canMove = true;
        hasAttacked = false;
        enemyStateMachine.StartCoroutine(enemyStateMachine.Cooldown("attack"));
    }
}
