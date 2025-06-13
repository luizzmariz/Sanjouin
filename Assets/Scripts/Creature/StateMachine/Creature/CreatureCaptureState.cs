using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCaptureState : BaseState
{
    SimpleCreatureStateMachine creatureStateMachine;

    public CreatureCaptureState(SimpleCreatureStateMachine stateMachine) : base("Damage", stateMachine) {
        creatureStateMachine = stateMachine;
    }

    public override void Enter() {
        // creatureStateMachine.canMove = false;

        // creatureStateMachine.canAttack = false;
        creatureStateMachine.beingPushed = true;
        creatureStateMachine.enemyDamageable.damageable = false;

        creatureStateMachine.StartCoroutine(Knockback());
    }

    public override void UpdateLogic() {
        Vector3 holderPosition = creatureStateMachine.transform.position;
        Vector3 playerPosition = creatureStateMachine.playerGameObject.transform.position;
        
        if(!creatureStateMachine.beingPushed)
        {
            if(Vector3.Distance(holderPosition, playerPosition) <= creatureStateMachine.rangeOfDanger)
            {
                stateMachine.ChangeState(creatureStateMachine.fleeState);
            }
            else
            {
                stateMachine.ChangeState(creatureStateMachine.idleState);
            }
        }
    }

    public override void UpdatePhysics() {
        
    }

    public IEnumerator Knockback()
    {
        Color previousColor =  creatureStateMachine.bodySpriteRenderer.color;

        creatureStateMachine.rigidBody.velocity = Vector3.zero;
        creatureStateMachine.rigidBody.AddForce(creatureStateMachine.knockbackVector, ForceMode2D.Impulse);

        creatureStateMachine.bodySpriteRenderer.color = new Color(previousColor.r, previousColor.g, previousColor.b, 0.5f);
        creatureStateMachine.handsSpriteRenderer.color = new Color(previousColor.r, previousColor.g, previousColor.b, 0.5f);

        yield return new WaitForSeconds(creatureStateMachine.knockbackDuration);

        creatureStateMachine.bodySpriteRenderer.color = previousColor;
        creatureStateMachine.handsSpriteRenderer.color = previousColor;

        creatureStateMachine.rigidBody.velocity = Vector3.zero;
        creatureStateMachine.beingPushed = false;
    }

    public override void Exit() 
    {
        creatureStateMachine.canMove = true;

        creatureStateMachine.enemyDamageable.damageable = true;
    }
}
