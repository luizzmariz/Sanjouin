using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : BaseState
{
    PlayerStateMachine playerStateMachine;

    public PlayerDeadState(PlayerStateMachine stateMachine) : base("Dead", stateMachine) {
        playerStateMachine = stateMachine;
    }

    public override void Enter() {
        playerStateMachine.canMove = false;
        playerStateMachine.canAttack = false;
        playerStateMachine.canDash = false;

        foreach(SpriteRenderer spriteRenderer in playerStateMachine.transform.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.enabled = false;
        }
    }

    public override void UpdateLogic() {
        
    }

    public override void UpdatePhysics() {
        playerStateMachine.rigidBody.velocity = Vector2.zero;
    }
}
