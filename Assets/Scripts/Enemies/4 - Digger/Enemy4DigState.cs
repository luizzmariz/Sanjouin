using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy4DigState : BaseState
{
    Enemy4StateMachine enemyStateMachine;

    Vector3 holderPosition;
    Vector3 playerPosition;

    bool hasDigged;
    Coroutine dig;
    bool isDigging;
    bool digWentWrong;

    List<Collider2D> colliders;
    ContactFilter2D contactFilter2D;
    

    public Enemy4DigState(Enemy4StateMachine stateMachine) : base("Run", stateMachine) 
    {
        enemyStateMachine = stateMachine;

        colliders = new List<Collider2D>();
        contactFilter2D = new ContactFilter2D
        {
            layerMask = LayerMask.GetMask("Collision"),
            useLayerMask = true
        };
    }

    public override void Enter() 
    {
        enemyStateMachine.enemyDamageable.damageable = false;
        isDigging = true;
    }

    public override void UpdateLogic() 
    {
        if(!isDigging)
        {
            hasDigged = false;
            if(digWentWrong)
            {
                stateMachine.ChangeState(enemyStateMachine.idleState);
            }
            else
            {
                stateMachine.ChangeState(enemyStateMachine.attackState);
            }
        }
    }

    public override void UpdatePhysics() 
    {
        if(!hasDigged)
        {
            enemyStateMachine.StartCoroutine(StartDig());
        }
        else
        {
            holderPosition = enemyStateMachine.transform.position;
            playerPosition = enemyStateMachine.playerGameObject.transform.position;

            Vector3 digDirection = (playerPosition - holderPosition).normalized;
            enemyStateMachine.rigidBody.velocity = digDirection * enemyStateMachine.digSpeed;

            if(Vector3.Distance(holderPosition, playerPosition) <= (enemyStateMachine.rangeOfAttack * 0.85))
            {
                enemyStateMachine.StopCoroutine(dig);
                enemyStateMachine.StartCoroutine(DigOut());
            }
            else if(enemyStateMachine.GetComponent<Collider2D>().OverlapCollider(contactFilter2D, colliders) > 0)
            {
                enemyStateMachine.StopCoroutine(dig);
                digWentWrong = true;
                enemyStateMachine.StartCoroutine(DigOut());
            }
        }
    }

    IEnumerator StartDig()
    {
        hasDigged = true;
        digWentWrong = false;
        enemyStateMachine.enemyDamageable.damageable = false;
        //enemyStateMachine.animator;

        yield return new WaitForSeconds(enemyStateMachine.diggingTime);

        dig = enemyStateMachine.StartCoroutine(DigTimer());
    }

    IEnumerator DigTimer()
    {
        yield return new WaitForSeconds(enemyStateMachine.maxDigDuration);

        digWentWrong = true;
        enemyStateMachine.StartCoroutine(DigOut());
    }

    IEnumerator DigOut()
    {
        yield return new WaitForSeconds(enemyStateMachine.diggingTime);

        isDigging = false;
        enemyStateMachine.enemyDamageable.damageable = true;
        enemyStateMachine.rigidBody.velocity = Vector3.zero;
    }

    public override void Exit() 
    {
        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        enemyStateMachine.enemyDamageable.damageable = true;
        enemyStateMachine.canDig = false;
        enemyStateMachine.StartCoroutine(enemyStateMachine.Cooldown("dig"));
    }
}
