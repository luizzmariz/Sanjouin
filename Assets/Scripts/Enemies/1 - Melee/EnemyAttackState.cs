using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : BaseState
{
    EnemyStateMachine enemyStateMachine;

    Vector3 holderPosition;
    Vector3 playerPosition;

    public EnemyAttackState(EnemyStateMachine stateMachine) : base("Charging", stateMachine)
    {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {
        base.Enter();

        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        //enemyStateMachine.animator.SetBool("chargingAttack", true);
        
    }

    public override void UpdateLogic() {
        holderPosition = enemyStateMachine.transform.position;
        playerPosition = enemyStateMachine.playerGameObject.transform.position;

        if(Vector3.Distance(holderPosition, playerPosition) > enemyStateMachine.rangeOfAttack)
        {
            if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfView)
            {
                stateMachine.ChangeState(enemyStateMachine.chaseState);
            }
            else
            {
                stateMachine.ChangeState(enemyStateMachine.idleState);
            }
            //enemyStateMachine.animator.SetBool("chargingAttack", false);
        }
    }

    public override void UpdatePhysics() {
        // enemyStateMachine.characterOrientation.ChangeOrientation(playerPosition);
    }

    public void Attack()
    {
        // enemyStateMachine.animator.SetBool("chargingAttack", false);
        // enemyStateMachine.animator.SetTrigger("castAttack");
        // enemyStateMachine.attackAnimator.SetTrigger("Attack");
    }

    public void AttackEnded()
    {
        // enemyStateMachine.animator.SetTrigger("attackEnd");
        stateMachine.ChangeState(enemyStateMachine.idleState);
        enemyStateMachine.attackCooldownTimer = enemyStateMachine.attackDuration;
    }
}
