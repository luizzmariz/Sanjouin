using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : BaseState
{
    PlayerStateMachine playerStateMachine;

    public PlayerIdleState(PlayerStateMachine stateMachine) : base("Idle", stateMachine) {
        playerStateMachine = stateMachine;
    }

    public override void Enter() {
        playerStateMachine.canMove = true;
        playerStateMachine.canAttack = true;
        playerStateMachine.canDash = true;
        playerStateMachine.playerDamageable.damageable = true;
    }

    public override void UpdateLogic() {
        Vector2 moveVector = playerStateMachine.playerInput.actions["move"].ReadValue<Vector2>();

        if(moveVector != Vector2.zero && !playerStateMachine.isAiming)
        {
            playerStateMachine.ChangeState(playerStateMachine.moveState);
        }
    }

    public override void UpdatePhysics() {
        if(playerStateMachine.isAiming)
        {
            Vector2 targetPoint = playerStateMachine.playerInput.actions["move"].ReadValue<Vector2>();
            targetPoint = new Vector2(playerStateMachine.transform.position.x + targetPoint.x, playerStateMachine.transform.position.y + targetPoint.y);
            playerStateMachine.characterOrientation.ChangeOrientation(targetPoint);
        }
    }
}