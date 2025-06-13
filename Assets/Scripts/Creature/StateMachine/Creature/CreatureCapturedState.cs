using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCapturedState : BaseState
{
    SimpleCreatureStateMachine creatureStateMachine;

    public CreatureCapturedState(SimpleCreatureStateMachine stateMachine) : base("Captured", stateMachine)
    {
        creatureStateMachine = stateMachine;
    }

    public override void Enter() 
    {
        if(creatureStateMachine.spawnedByRegularLogic)
        {
            creatureStateMachine.creatureSpawner.CreatureCaptured(creatureStateMachine.gameObject);
        }
        
        Object.Destroy(creatureStateMachine.gameObject);
    }

    public override void UpdateLogic() {
        
    }

    public override void UpdatePhysics() {

    }
}
