using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : BaseState
{
    PlayerStateMachine playerStateMachine;

    public PlayerDeadState(PlayerStateMachine stateMachine) : base("Dead", stateMachine) {
        playerStateMachine = stateMachine;
    }

    public override void Enter() {
        playerStateMachine.canMove = false;
        playerStateMachine.canAttack = false;
        playerStateMachine.canDash = false;
        playerStateMachine.playerDamageable.damageable = false;
        
        Debug.Log("GG MORREU");
    }

    public override void UpdateLogic() {
        
    }

    public override void UpdatePhysics() {

    }
}
