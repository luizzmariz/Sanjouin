using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Properties;
using UnityEditor;
using UnityEngine;

public class PlayerFireState : BaseState
{
  PlayerStateMachine playerStateMachine;



    public PlayerFireState(PlayerStateMachine stateMachine) : base("Attack", stateMachine) {
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

            if(moveVector != Vector2.zero)
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
            Vector2 lookDirection = playerStateMachine.playerInput.actions["look"].ReadValue<Vector2>();

            targetPoint = new Vector3(targetPoint.x + lookDirection.x * 10, targetPoint.y + lookDirection.y * 10, targetPoint.z);
            playerStateMachine.characterOrientation.ChangeOrientation(targetPoint);
        }
        Vector3 bulletDirection = (targetPoint - playerStateMachine.transform.position).normalized;
        GameObject intBullet = GameObject.Instantiate(playerStateMachine.Bullet, playerStateMachine.transform.position, Quaternion.identity);
        intBullet.GetComponent<Rigidbody2D>().AddForce(bulletDirection * playerStateMachine.fireForce, ForceMode2D.Impulse);
        GameObject.Destroy(intBullet, 2f);
        yield return new WaitForSeconds(playerStateMachine.attackDuration);
        

        playerStateMachine.rigidBody.velocity = Vector2.zero;
        playerStateMachine.isAttacking = false;
    
    }

}
