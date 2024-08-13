using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2FleeState : BaseState
{
    Enemy2StateMachine enemyStateMachine;

    public Vector3 holderPosition;
    public Vector3 playerPosition;

    public Enemy2FleeState(Enemy2StateMachine stateMachine) : base("Flee", stateMachine) {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {
        enemyStateMachine.canMove = true;
        enemyStateMachine.enemyDamageable.damageable = true;
    }

    public override void UpdateLogic() {
        holderPosition = enemyStateMachine.transform.position;
        playerPosition = enemyStateMachine.playerGameObject.transform.position;

        if(Vector3.Distance(holderPosition, playerPosition) > enemyStateMachine.rangeOfDanger)
        {
            enemyStateMachine.rigidBody.velocity = Vector3.zero;
            
            // followingPath = false;
            // path = null;

            if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfAttack)
            {
                if(enemyStateMachine.canAttack)
                {
                    stateMachine.ChangeState(enemyStateMachine.attackState);
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

    }
}
