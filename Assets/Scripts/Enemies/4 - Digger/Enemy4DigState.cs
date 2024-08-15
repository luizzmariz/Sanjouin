using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy4DigState : BaseState
{
    Enemy4StateMachine enemyStateMachine;

    Vector3 holderPosition;
    Vector3 playerPosition;

    bool hasStartedRunning;
    Coroutine run;
    bool isRunning;

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
        isRunning = true;
    }

    public override void UpdateLogic() 
    {
        if(!isRunning)
        {
            holderPosition = enemyStateMachine.transform.position;
            playerPosition = enemyStateMachine.playerGameObject.transform.position;
            
            if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfAttack)
            {
                if(enemyStateMachine.canAttack)
                {
                    stateMachine.ChangeState(enemyStateMachine.attackState);
                }
                enemyStateMachine.characterOrientation.ChangeOrientation(playerPosition);
            }
            else if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfView)
            {
                stateMachine.ChangeState(enemyStateMachine.chaseState);
            }
        }
    }

    public override void UpdatePhysics() 
    {
        if(!hasStartedRunning)
        {
            StartRun();
        }
        else
        {
            if(enemyStateMachine.GetComponent<Collider2D>().OverlapCollider(contactFilter2D, colliders) > 0)
            {
                enemyStateMachine.StopCoroutine(run);
                enemyStateMachine.rigidBody.velocity = Vector3.zero;
                hasStartedRunning = false;
                isRunning = false;
            }
        }
    }

    void StartRun()
    {
        hasStartedRunning = true;
        run = enemyStateMachine.StartCoroutine(RunTimer());
        //enemyStateMachine.animator;

        holderPosition = enemyStateMachine.transform.position;
        playerPosition = enemyStateMachine.playerGameObject.transform.position;

        Vector3 runVector = (playerPosition - holderPosition).normalized;
        enemyStateMachine.rigidBody.velocity = runVector * enemyStateMachine.runSpeed;
    }

    IEnumerator RunTimer()
    {
        yield return new WaitForSeconds(enemyStateMachine.maxRunDuration);
        //enemyStateMachine.animator;
        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        hasStartedRunning = false;
        isRunning = false;
    }

    public override void Exit() 
    {
        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        enemyStateMachine.enemyDamageable.damageable = true;
        enemyStateMachine.canDig = false;
        enemyStateMachine.StartCoroutine(enemyStateMachine.Cooldown("run"));
    }
}
