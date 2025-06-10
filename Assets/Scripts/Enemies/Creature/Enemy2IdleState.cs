using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2IdleState : BaseState
{
    SimpleCreatureStateMachine enemyStateMachine;

    public Enemy2IdleState(SimpleCreatureStateMachine stateMachine) : base("Idle", stateMachine) {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {
        enemyStateMachine.canMove = true;
        enemyStateMachine.enemyDamageable.damageable = true;
    }

    public override void UpdateLogic() {
        if(!(enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().currentState == enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().deadState))
        {
            Vector3 holderPosition = enemyStateMachine.transform.position;
            Vector3 playerPosition = enemyStateMachine.playerGameObject.transform.position;
            
            if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfDanger && enemyStateMachine.canFlee)
            {
                stateMachine.ChangeState(enemyStateMachine.fleeState);
            }
            else if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfView)
            {
                stateMachine.ChangeState(enemyStateMachine.chaseState);
            }
        }
    }

    public override void UpdatePhysics() {
        enemyStateMachine.rigidBody.velocity = Vector2.zero;
    }

    public override void Exit() 
    {
        
    }
}
