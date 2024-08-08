using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageState : BaseState
{
    PlayerStateMachine playerStateMachine;

    public PlayerDamageState(PlayerStateMachine stateMachine) : base("Damage", stateMachine) {
        playerStateMachine = stateMachine;
    }

    public override void Enter() {
        playerStateMachine.canMove = false;
        playerStateMachine.canAttack = false;
        playerStateMachine.canDash = false;
        playerStateMachine.beingPushed = true;
        playerStateMachine.playerDamageable.damageable = false;

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

        yield return new WaitForSeconds(playerStateMachine.knockbackDuration);

        playerStateMachine.rigidBody.velocity = Vector3.zero;
        playerStateMachine.beingPushed = false;
    }
}