using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy3RunState : BaseState
{
    Enemy3StateMachine enemyStateMachine;

    Vector3 holderPosition;
    Vector3 playerPosition;

    bool hasStartedRunning;
    Coroutine run;
    bool isRunning;

    List<Collider2D> scenarioColliders;
    ContactFilter2D scenarioContactFilter2D;
    List<Collider2D> damageableCollider;
    List<Collider2D> usedDamageableCollider;
    ContactFilter2D damageableContactFilter2D;
    

    public Enemy3RunState(Enemy3StateMachine stateMachine) : base("Run", stateMachine) 
    {
        enemyStateMachine = stateMachine;

        scenarioColliders = new List<Collider2D>();
        scenarioContactFilter2D = new ContactFilter2D
        {
            layerMask = LayerMask.GetMask("Collision"),
            useLayerMask = true
        };

        damageableCollider = new List<Collider2D>();
        damageableContactFilter2D = new ContactFilter2D
        {
            layerMask = LayerMask.GetMask("Damageable"),
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
            if(enemyStateMachine.GetComponent<Collider2D>().OverlapCollider(scenarioContactFilter2D, scenarioColliders) > 0)
            {
                enemyStateMachine.StopCoroutine(run);
                enemyStateMachine.rigidBody.velocity = Vector3.zero;
                hasStartedRunning = false;
                isRunning = false;
            }
            if(enemyStateMachine.runCollider.OverlapCollider(damageableContactFilter2D, damageableCollider) > 0)
            {
                foreach(Collider2D collider in damageableCollider)
                {
                    if(!usedDamageableCollider.Contains(collider))
                    {
                        if(collider.GetComponent<PlayerDamageable>())
                        {
                            collider.GetComponent<PlayerDamageable>().Damage(enemyStateMachine.runDamage, enemyStateMachine.transform.position);
                        }
                    }
                    usedDamageableCollider.Add(collider);
                }
            }
        }
    }

    void StartRun()
    {
        hasStartedRunning = true;
        run = enemyStateMachine.StartCoroutine(RunTimer());
        //enemyStateMachine.animator;
        usedDamageableCollider = new List<Collider2D>();

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
        enemyStateMachine.canRun = false;
        usedDamageableCollider = null;
        enemyStateMachine.StartCoroutine(enemyStateMachine.Cooldown("run"));
    }
}
