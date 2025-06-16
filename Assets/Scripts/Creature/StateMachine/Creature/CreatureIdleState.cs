using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureIdleState : BaseState
{
    SimpleCreatureStateMachine creatureStateMachine;

    public CreatureIdleState(SimpleCreatureStateMachine stateMachine) : base("Idle", stateMachine)
    {
        creatureStateMachine = stateMachine;
    }

    public override void Enter()
    {
        // creatureStateMachine.canMove = true;
        // creatureStateMachine.enemyDamageable.damageable = true;
    }

    public override void UpdateLogic()
    {
        Vector3 holderPosition = creatureStateMachine.transform.position;
        Vector3 playerPosition = creatureStateMachine.playerGameObject.transform.position;

        if (Vector3.Distance(holderPosition, playerPosition) <= creatureStateMachine.rangeOfDanger && creatureStateMachine.canFlee)
        {
            stateMachine.ChangeState(creatureStateMachine.fleeState);
        }
        else if (creatureStateMachine.canRoam)
        {
            stateMachine.ChangeState(creatureStateMachine.roamState);
        }
    }

    public override void UpdatePhysics()
    {
        creatureStateMachine.rigidBody.velocity = Vector2.zero;
    }

    public override void Exit() 
    {
        
    }
}
