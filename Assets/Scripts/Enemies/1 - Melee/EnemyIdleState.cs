using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : BaseState
{
    EnemyStateMachine enemyStateMachine;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base("Idle", stateMachine) {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {
        enemyStateMachine.canMove = true;
        // enemyStateMachine.canAttack = true;
        enemyStateMachine.enemyDamageable.damageable = true;
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

    public override void UpdatePhysics() {

    }
}
