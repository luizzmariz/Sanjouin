using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : BaseState
{
    PlayerStateMachine playerStateMachine;

    bool couldMove;
    bool couldAttack;
    bool couldFire;

    bool dashGoingOn;

    public PlayerDashState(PlayerStateMachine stateMachine) : base("Dash", stateMachine) {
        playerStateMachine = stateMachine;
    }

    public override void Enter() {
        // playerStateMachine.rigidBody.velocity = Vector3.zero;
        couldMove = false;
        couldAttack = false;
        couldFire = false;

        if(playerStateMachine.canMove)
        {
            couldMove = true;
        }
        playerStateMachine.canMove = false;
        if(playerStateMachine.canAttack 
        //|| playerStateMachine.runningCoroutines.Contains("attack")
        )
        {
            couldAttack = true;
        }
        playerStateMachine.canAttack = false;
        if(playerStateMachine.canFire 
        //|| playerStateMachine.runningCoroutines.Contains("fire")
        )
        {
            couldFire = true;
        }
        playerStateMachine.canFire = false;
        // Debug.Log("is starting to dash, canDash: " + playerStateMachine.canDash + ". Time: " + Time.time);
        playerStateMachine.canDash = false;
        playerStateMachine.isDashing = true;
        dashGoingOn = true;
        playerStateMachine.StartCoroutine(Dash());
    }

    public override void UpdateLogic() {
        // if(!playerStateMachine.isDashing)
        if(!dashGoingOn)
        {
            playerStateMachine.isDashing = false;
            playerStateMachine.StartCoroutine(playerStateMachine.Cooldown("dash"));

            // Debug.Log("Stopped Dashing, calling coroutine" + ". Time: " + Time.time);
            // playerStateMachine.StartCoroutine(playerStateMachine.DashCooldown());
            // playerStateMachine.runningCoroutines.Add("dash");

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

        //Debug.Log("dashing part 1, canDash: " + playerStateMachine.canDash + ", isDashing: " + playerStateMachine.isDashing + ", canAttack: " + playerStateMachine.canAttack + ". Time: " + Time.time);
        yield return new WaitForSeconds(playerStateMachine.dashingTime);

        // playerStateMachine.trailRenderer.emitting = false;
        playerStateMachine.rigidBody.velocity = Vector2.zero;
        dashGoingOn = false;
        //Debug.Log("dashing part 2, canDash: " + playerStateMachine.canDash + ", isDashing: " + playerStateMachine.isDashing + ", canAttack: " + playerStateMachine.canAttack + ". Time: " + Time.time);
    }

    public override void Exit()
    {
        playerStateMachine.isDashing = false;
        if(couldMove)
        {
            playerStateMachine.canMove = true;
        }
        if(couldAttack 
        //|| playerStateMachine.canAttack
        )
        {
            //Debug.Log("dashing part 2, canDash: " + playerStateMachine.canDash + ", isDashing: " + playerStateMachine.isDashing + ", canAttack: " + playerStateMachine.canAttack + ". Time: " + Time.time);
            playerStateMachine.canAttack = true;
        }
        if(couldFire 
        //|| playerStateMachine.canFire
        )
        {
            playerStateMachine.canFire = true;
        }
    }
}