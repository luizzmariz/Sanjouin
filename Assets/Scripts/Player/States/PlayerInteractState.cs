using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractState : BaseState
{
    PlayerStateMachine playerStateMachine;
    
    public PlayerInteractState(PlayerStateMachine stateMachine) : base("Interact", stateMachine)
    {
        playerStateMachine = stateMachine;
    }

    public override void Enter() {

    }

    public override void UpdateLogic() {
        if(!playerStateMachine.isInteracting)
        {
            playerStateMachine.ChangeState(playerStateMachine.idleState);
        }
    }

    public override void UpdatePhysics() {
        playerStateMachine.rigidBody.velocity = Vector2.zero;
    }

    public void ExitState()
    {

    }
}
