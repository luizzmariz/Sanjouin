using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCatchState : BaseState
{
    SimpleCreatureStateMachine creatureStateMachine;

    public CreatureCatchState(SimpleCreatureStateMachine stateMachine) : base("Damage", stateMachine) {
        creatureStateMachine = stateMachine;
    }

    public override void Enter() {

    }

    public override void UpdateLogic() {
        if(!creatureStateMachine.stopped)
        {
            Vector3 holderPosition = creatureStateMachine.transform.position;
            Vector3 playerPosition = creatureStateMachine.playerGameObject.transform.position;
            
            if (Vector3.Distance(holderPosition, playerPosition) <= creatureStateMachine.rangeOfDanger)
            {
                creatureStateMachine.ChangeState(creatureStateMachine.fleeState);
            }
            else
            {
                creatureStateMachine.ChangeState(creatureStateMachine.idleState);
            }
        }
    }

    public override void UpdatePhysics() {
        creatureStateMachine.rigidBody.velocity = Vector2.zero;
    }

    public override void Exit() 
    {
        creatureStateMachine.canMove = true;
    }
}
