using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1DamageState : BaseState
{
    Enemy1StateMachine enemyStateMachine;

    bool shouldTurnAttackOn;

    public Enemy1DamageState(Enemy1StateMachine stateMachine) : base("Damage", stateMachine) {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {
        enemyStateMachine.canMove = false;
        if(enemyStateMachine.canAttack)
        {
            shouldTurnAttackOn = true;
        }
        enemyStateMachine.canAttack = false;
        enemyStateMachine.beingPushed = true;
        enemyStateMachine.enemyDamageable.damageable = false;

        enemyStateMachine.StartCoroutine(Knockback());
    }

    public override void UpdateLogic() {
        Vector3 holderPosition = enemyStateMachine.transform.position;
        Vector3 playerPosition = enemyStateMachine.playerGameObject.transform.position;
        
        if(!enemyStateMachine.beingPushed)
        {
            if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfAttack)
            {
                if(enemyStateMachine.canAttack)
                {
                    stateMachine.ChangeState(enemyStateMachine.attackState);
                }
            }
            else if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfView)
            {
                stateMachine.ChangeState(enemyStateMachine.chaseState);
            }
            else
            {
                stateMachine.ChangeState(enemyStateMachine.idleState);
            }
        }
    }

    public override void UpdatePhysics() {
        
    }

    public IEnumerator Knockback()
    {
        Color previousColor =  enemyStateMachine.bodySpriteRenderer.color;

        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        enemyStateMachine.rigidBody.AddForce(enemyStateMachine.knockbackVector, ForceMode2D.Impulse);

        enemyStateMachine.bodySpriteRenderer.color = new Color(previousColor.r, previousColor.g, previousColor.b, 0.5f);
        enemyStateMachine.handsSpriteRenderer.color = new Color(previousColor.r, previousColor.g, previousColor.b, 0.5f);

        yield return new WaitForSeconds(enemyStateMachine.knockbackDuration);

        enemyStateMachine.bodySpriteRenderer.color = previousColor;
        enemyStateMachine.handsSpriteRenderer.color = previousColor;

        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        enemyStateMachine.beingPushed = false;
    }

    public override void Exit() 
    {
        enemyStateMachine.canMove = true;
        if(shouldTurnAttackOn)
        {
            enemyStateMachine.canAttack = true;
        }
        enemyStateMachine.enemyDamageable.damageable = true;
    }
}
