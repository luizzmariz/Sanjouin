using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageState : BaseState
{
    PlayerStateMachine playerStateMachine;

    bool couldMove;
    bool couldAttack;
    bool couldFire;
    bool couldDash;

    public PlayerDamageState(PlayerStateMachine stateMachine) : base("Damage", stateMachine) {
        playerStateMachine = stateMachine;
    }

    public override void Enter() {
        couldMove = false;
        couldAttack = false;
        couldFire = false;
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
        if(playerStateMachine.canFire)
        {
            couldFire = true;
        }
        playerStateMachine.canFire = false;
        if(playerStateMachine.canDash)
        {
            couldDash = true;
        }
        playerStateMachine.canDash = false;
        playerStateMachine.beingPushed = true;

        playerStateMachine.StartCoroutine(Knockback());
    }

    public override void UpdateLogic() {
        if(!playerStateMachine.beingPushed)
        {
            playerStateMachine.ChangeState(playerStateMachine.idleState);
        }
    }

    public override void UpdatePhysics() {

    }

    public IEnumerator Knockback()
    {
        playerStateMachine.rigidBody.velocity = Vector3.zero;
        playerStateMachine.rigidBody.AddForce(playerStateMachine.knockbackVector, ForceMode2D.Impulse);
        playerStateMachine.bodySpriteRenderer.color = new Color(1, 1, 1, 0.5f);
        playerStateMachine.handsSpriteRenderer.color = new Color(1, 1, 1, 0.5f);

        yield return new WaitForSeconds(playerStateMachine.knockbackDuration);

        playerStateMachine.bodySpriteRenderer.color = new Color(1, 1, 1, 1f);
         playerStateMachine.handsSpriteRenderer.color = new Color(1, 1, 1, 1f);

        playerStateMachine.rigidBody.velocity = Vector3.zero;
        playerStateMachine.beingPushed = false;
    }

    public override void Exit()
    {
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
        if(couldDash)
        {
            playerStateMachine.canDash = true;
        }
    }
}