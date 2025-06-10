using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2DeadState : BaseState
{
    SimpleCreatureStateMachine enemyStateMachine;

    public Enemy2DeadState(SimpleCreatureStateMachine stateMachine) : base("Dead", stateMachine)
    {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() 
    {
        if(enemyStateMachine.spawnedInWave)
        {
            enemyStateMachine.waveSpawner.EnemyDied();
        }
        
        Object.Destroy(enemyStateMachine.gameObject);
    }

    public override void UpdateLogic() {
        
    }

    public override void UpdatePhysics() {

    }
}
