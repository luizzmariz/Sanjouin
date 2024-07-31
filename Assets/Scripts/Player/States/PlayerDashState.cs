using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : BaseState
{
    PlayerStateMachine playerStateMachine;

    public PlayerDashState(PlayerStateMachine stateMachine) : base("Dash", stateMachine) {
        playerStateMachine = stateMachine;
    }

    public override void Enter() {
        // playerStateMachine.rigidBody.velocity = Vector3.zero;

        playerStateMachine.canMove = false;
        playerStateMachine.canAttack = false;
        playerStateMachine.canDash = false;
        playerStateMachine.isDashing = true;
        playerStateMachine.StartCoroutine(Dash());
    }

    public override void UpdateLogic() {
        if(!playerStateMachine.isDashing)
        {
            playerStateMachine.StartCoroutine(playerStateMachine.Cooldown("dash"));

            playerStateMachine.ChangeState(playerStateMachine.idleState);
        }
    }

    public override void UpdatePhysics() {

    }

    public IEnumerator Dash()
    {
        //Vector2 dashDirection = playerStateMachine.playerInput.actions["move"].ReadValue<Vector2>();
        Vector2 dashDirection = playerStateMachine.playerOrientation.lastOrientation;
        if(dashDirection == Vector2.zero)
        {
            dashDirection = Vector2.down;
        }
        playerStateMachine.rigidBody.velocity = dashDirection.normalized * playerStateMachine.dashingPower;
        // playerStateMachine.trailRenderer.emitting = true;

        yield return new WaitForSeconds(playerStateMachine.dashingTime);

        // playerStateMachine.trailRenderer.emitting = false;
        playerStateMachine.rigidBody.velocity = Vector2.zero;
        playerStateMachine.isDashing = false;
        // playerStateMachine.canMove = true;
        // playerStateMachine.canAttack = true;
        // playerStateMachine.rigidBody.velocity = Vector3.zero;

        // yield return new WaitForSeconds(playerStateMachine.dashCooldownTimer);

        // playerStateMachine.canDash = true;
    }
}