using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2DeadState : BaseState
{
    Enemy2StateMachine enemyStateMachine;

    public Enemy2DeadState(Enemy2StateMachine stateMachine) : base("Dead", stateMachine)
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
