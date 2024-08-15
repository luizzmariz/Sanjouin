using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3IdleState : BaseState
{
    Enemy3StateMachine enemyStateMachine;

    public Enemy3IdleState(Enemy3StateMachine stateMachine) : base("Idle", stateMachine) {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {

    }

    public override void UpdateLogic() {
        Vector3 holderPosition = enemyStateMachine.transform.position;
        Vector3 playerPosition = enemyStateMachine.playerGameObject.transform.position;
        
        if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfAttack)
        {
            if(enemyStateMachine.canAttack)
            {
                stateMachine.ChangeState(enemyStateMachine.attackState);
            }
            enemyStateMachine.characterOrientation.ChangeOrientation(playerPosition);
        }
        else if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfView)
        {
            stateMachine.ChangeState(enemyStateMachine.chaseState);
        }
    }

    public override void UpdatePhysics()
    {
    
    }

    public override void Exit() 
    {
        
    }
}
