using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : BaseState
{
    PlayerStateMachine playerStateMachine;

    bool couldMove;
    bool couldAttack;
    bool couldFire;

    public PlayerDashState(PlayerStateMachine stateMachine) : base("Dash", stateMachine) {
        playerStateMachine = stateMachine;
    }

    public override void Enter() {
        // playerStateMachine.rigidBody.velocity = Vector3.zero;

        if(playerStateMachine.canMove)
        {
            couldMove = true;
        }
        playerStateMachine.canMove = false;
        if(playerStateMachine.canAttack)
        {
            couldAttack = true;
        }
        playerStateMachine.canAttack = false;
        if(playerStateMachine.canFire)
        {
            couldFire = true;
        }
        playerStateMachine.canFire = false;
        playerStateMachine.canDash = false;
        playerStateMachine.isDashing = true;
        playerStateMachine.playerDamageable.damageable = false;
        playerStateMachine.StartCoroutine(Dash());
    }

    public override void UpdateLogic() {
        if(!playerStateMachine.isDashing)
        {
            playerStateMachine.StartCoroutine(playerStateMachine.Cooldown("dash"));

            Vector2 moveVector = playerStateMachine.playerInput.actions["move"].ReadValue<Vector2>();

            if(moveVector != Vector2.zero// && playerStateMachine.canMove <-- qual a utilidade disso? não funcionaria por que há uma linha nesse script que faz canMove = false. REAVALIAR ESSAS VARIAVEIS
            )
            {
                playerStateMachine.ChangeState(playerStateMachine.moveState);
            }
            else
            {
                playerStateMachine.ChangeState(playerStateMachine.idleState);
            }
        }
    }

    public override void UpdatePhysics() {

    }

    public IEnumerator Dash()
    {
        //Vector2 dashDirection = playerStateMachine.playerInput.actions["move"].ReadValue<Vector2>();
        Vector2 dashDirection = playerStateMachine.characterOrientation.lastOrientation;
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

    public override void Exit()
    {
        playerStateMachine.playerDamageable.damageable = true;
        if(couldMove)
        {
            playerStateMachine.canMove = true;
        }
        if(couldAttack)
        {
            playerStateMachine.canAttack = true;
        }
        if(couldFire)
        {
            playerStateMachine.canFire = true;
        }
    }
}