using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Properties;
using UnityEditor;
using UnityEngine;

public class PlayerFireState : BaseState
{
    PlayerStateMachine playerStateMachine;

    bool hasAttacked;

    bool couldMove;
    bool couldAttack;
    bool couldDash;

    public PlayerFireState(PlayerStateMachine stateMachine) : base("Attack", stateMachine) {
        playerStateMachine = stateMachine;
    }

    public override void Enter() {
        playerStateMachine.rigidBody.velocity = Vector3.zero;
        couldMove = false;
        couldAttack = false;
        couldDash = false;

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
        if(playerStateMachine.canDash)
        {
            couldDash = true;
        }
        playerStateMachine.canDash = false;
        playerStateMachine.canFire = false;
        playerStateMachine.isAttacking = true;
        playerStateMachine.attacked = true;
        hasAttacked = false;
    }

    public override void UpdateLogic() {
        if(!playerStateMachine.attacked)
        {
            playerStateMachine.StartCoroutine(playerStateMachine.Cooldown("fire"));
            playerStateMachine.isAttacking = false;

            Vector2 moveVector = playerStateMachine.playerInput.actions["move"].ReadValue<Vector2>();

            if(moveVector != Vector2.zero && !playerStateMachine.isAiming)
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
        if(!hasAttacked)
        {
            Vector3 targetPoint = playerStateMachine.transform.position;

            if(playerStateMachine.playerInput.currentControlScheme == "Keyboard&Mouse")
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                targetPoint = new Vector3(mousePos.x, mousePos.y, targetPoint.z);
                playerStateMachine.characterOrientation.ChangeOrientation(targetPoint);
            }
            else if(playerStateMachine.playerInput.currentControlScheme == "Gamepad")
            {
                Vector2 lookDirection = playerStateMachine.characterOrientation.lastOrientation;
                if(playerStateMachine.isAiming)
                {
                    lookDirection = playerStateMachine.playerInput.actions["move"].ReadValue<Vector2>();
                }

                targetPoint = new Vector3(targetPoint.x + lookDirection.x * 10, targetPoint.y + lookDirection.y * 10, targetPoint.z);
                playerStateMachine.characterOrientation.ChangeOrientation(targetPoint);
            }

            Vector3 attackDirection = (targetPoint - playerStateMachine.transform.position).normalized;

            playerStateMachine.playerHands.Attack(attackDirection, 1);
            hasAttacked = true;
        }
    }

    public override void Exit() 
    {
        hasAttacked = false;

        if(couldMove)
        {
            playerStateMachine.canMove = true;
        }
        if(couldAttack)
        {
            playerStateMachine.canAttack = true;
        }
        if(couldDash)
        {
            playerStateMachine.canDash = true;
        }
    }
}
