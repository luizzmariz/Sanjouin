using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : BaseState
{
    PlayerStateMachine playerStateMachine;



    public PlayerAttackState(PlayerStateMachine stateMachine) : base("Attack", stateMachine) {
        playerStateMachine = stateMachine;
    }

    public override void Enter() {
        // playerStateMachine.rigidBody.velocity = Vector3.zero;
        playerStateMachine.canMove = false;
        playerStateMachine.canAttack = false;
        playerStateMachine.isAttacking = true;
        // if(playerStateMachine.attackType == 1)
        // {
        //     playerStateMachine.attack1CooldownTimer = playerStateMachine.attackDuration;

        // }
        // else if(playerStateMachine.attackType == 2)
        // {
        //     playerStateMachine.attack2CooldownTimer = playerStateMachine.attackDuration;

        // }
        // SetAttack();
        playerStateMachine.StartCoroutine(SetAttack());
    }

    public override void UpdateLogic() {
        if(!playerStateMachine.isAttacking)
        {
            playerStateMachine.StartCoroutine(playerStateMachine.Cooldown("attack"));
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

    }

    public IEnumerator SetAttack()
    {
        //playerStateMachine.rigidBody.velocity = Vector2.zero;
        playerStateMachine.rigidBody.velocity = playerStateMachine.rigidBody.velocity/2;
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

        playerStateMachine.animator.SetTrigger("playerAttack");

        playerStateMachine.melee.SetActive(true);
        playerStateMachine.melee.GetComponent<PlayerHands>().ExecuteAttack();

        yield return new WaitForSeconds(playerStateMachine.attackDuration);

        playerStateMachine.melee.GetComponent<PlayerHands>().StopAttack();
        playerStateMachine.melee.SetActive(false);

        playerStateMachine.rigidBody.velocity = Vector2.zero;
        playerStateMachine.isAttacking = false;
    }
}