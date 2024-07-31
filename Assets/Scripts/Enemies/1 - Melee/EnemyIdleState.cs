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
        // enemyStateMachine.canMove = true;
        // enemyStateMachine.canAttack = true;
        // enemyStateMachine.canDash = true;
    }

    public override void UpdateLogic() {
        // Vector2 moveVector = enemyStateMachine.playerInput.actions["move"].ReadValue<Vector2>();

        // if(moveVector != Vector2.zero)
        // {
        //     enemyStateMachine.ChangeState(enemyStateMachine.moveState);
        // }
    }

    public override void UpdatePhysics() {

    }
}
