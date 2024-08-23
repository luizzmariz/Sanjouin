using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4AttackState : BaseState
{
    Enemy4StateMachine enemyStateMachine;

    Vector3 holderPosition;
    Vector3 playerPosition;

    bool hasAttacked;

    public Enemy4AttackState(Enemy4StateMachine stateMachine) : base("Attacking", stateMachine)
    {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() 
    {
        enemyStateMachine.rigidBody.velocity = Vector3.zero;

        enemyStateMachine.canMove = false;
        enemyStateMachine.canAttack = false;
        enemyStateMachine.isAttacking = true;
        hasAttacked = false;
    }

    public override void UpdateLogic() {
        if(!enemyStateMachine.isAttacking)
        {
            if(enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().currentState == enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().deadState)
            {
                stateMachine.ChangeState(enemyStateMachine.idleState);
            }
            
            holderPosition = enemyStateMachine.transform.position;
            playerPosition = enemyStateMachine.playerGameObject.transform.position;

            stateMachine.ChangeState(enemyStateMachine.fleeState);
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
