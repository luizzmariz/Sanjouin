using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4DeadState : BaseState
{
    Enemy4StateMachine enemyStateMachine;

    public Enemy4DeadState(Enemy4StateMachine stateMachine) : base("Dead", stateMachine)
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

    public override void Exit() 
    {
        
    }
}
