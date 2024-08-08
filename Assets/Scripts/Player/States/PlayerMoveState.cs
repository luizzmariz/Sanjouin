using UnityEngine;

public class PlayerMoveState : BaseState
{
    PlayerStateMachine playerStateMachine;
    Vector2 moveVector;

    public PlayerMoveState(PlayerStateMachine stateMachine) : base("Move", stateMachine) {
        playerStateMachine = stateMachine;
    }

    public override void Enter() {
        playerStateMachine.canAttack = true;
        playerStateMachine.canDash = true;
    }

    public override void UpdateLogic() {
        moveVector = playerStateMachine.playerInput.actions["move"].ReadValue<Vector2>();

        if(moveVector == Vector2.zero)
        {
            playerStateMachine.rigidBody.velocity = Vector3.zero;
            // playerStateMachine.animator.SetBool("isMoving", false);
            playerStateMachine.ChangeState(playerStateMachine.idleState);
        }
    }

    public override void UpdatePhysics() {
        Move();
        SendOrientation();

        // playerStateMachine.animator.SetBool("isMoving", true);
    }

    void Move()
    {
        playerStateMachine.rigidBody.velocity = moveVector.normalized * playerStateMachine.movementSpeed;
    }

    void SendOrientation()
    {
        if(!playerStateMachine.isAttacking)
        {
            Vector2 orientation = new Vector2(playerStateMachine.transform.position.x + moveVector.x, playerStateMachine.transform.position.y + moveVector.y);
            playerStateMachine.characterOrientation.ChangeOrientation(orientation);
        }
    }
}