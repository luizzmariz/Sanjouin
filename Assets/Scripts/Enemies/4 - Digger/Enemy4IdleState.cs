using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4IdleState : BaseState
{
    Enemy4StateMachine enemyStateMachine;

    Vector3 holderPosition;
    Vector3 playerPosition;

    public Enemy4IdleState(Enemy4StateMachine stateMachine) : base("Idle", stateMachine) {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {

    }

    public override void UpdateLogic() {
        if(!(enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().currentState == enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().deadState))
        {
            holderPosition = enemyStateMachine.transform.position;
            playerPosition = enemyStateMachine.playerGameObject.transform.position;
            
            if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfDig)
            {
                if(enemyStateMachine.canDig)
                {
                    stateMachine.ChangeState(enemyStateMachine.digState);
                }
                else
                {
                    stateMachine.ChangeState(enemyStateMachine.fleeState);
                }
            }
            else if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfView)
            {
                if(enemyStateMachine.canDig)
                {
                    stateMachine.ChangeState(enemyStateMachine.chaseState);
                }   
            }
        }
    }

    public override void UpdatePhysics()
    {
        enemyStateMachine.rigidBody.velocity = Vector2.zero;
        
        holderPosition = enemyStateMachine.transform.position;
        playerPosition = enemyStateMachine.playerGameObject.transform.position;

        if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfView)
        {
            Vector3 playerDirection = (playerPosition - holderPosition).normalized * 5;
            enemyStateMachine.characterOrientation.ChangeOrientation(playerDirection);
        }
    }

    public override void Exit() 
    {
        
    }
}
