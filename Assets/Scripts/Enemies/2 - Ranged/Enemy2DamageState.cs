using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2DamageState : BaseState
{
    Enemy2StateMachine enemyStateMachine;

    public Enemy2DamageState(Enemy2StateMachine stateMachine) : base("Damage", stateMachine) {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {
        enemyStateMachine.canMove = false;
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
        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        enemyStateMachine.rigidBody.AddForce(enemyStateMachine.knockbackVector, ForceMode2D.Impulse);

        yield return new WaitForSeconds(enemyStateMachine.knockbackDuration);

        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        enemyStateMachine.beingPushed = false;
    }
}
