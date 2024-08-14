using System.Linq;
using UnityEngine;

public class Enemy2FleeState : BaseState
{
    Enemy2StateMachine enemyStateMachine;

    public Vector3 holderPosition;
    public Vector3 playerPosition;
    public bool hasAskedPath;
    public bool followingPath;

    int targetIndex;
    public Vector3[] path;

    public Enemy2FleeState(Enemy2StateMachine stateMachine) : base("Flee", stateMachine) {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {
        enemyStateMachine.canMove = true;
        enemyStateMachine.enemyDamageable.damageable = true;
        enemyStateMachine.isFleeing = true;
    }

    public override void UpdateLogic() {
        holderPosition = enemyStateMachine.transform.position;
        playerPosition = enemyStateMachine.playerGameObject.transform.position;
        
        if(Vector3.Distance(holderPosition, playerPosition) > enemyStateMachine.rangeOfAttack)
        {

            stateMachine.ChangeState(enemyStateMachine.idleState);
        }
        else if(!enemyStateMachine.isFleeing)
        {
            if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfAttack)
            {
                if(enemyStateMachine.canAttack)
                {
                    stateMachine.ChangeState(enemyStateMachine.attackState);
                }
                else
                {
                    stateMachine.ChangeState(enemyStateMachine.idleState);
                }
            }
            else
            {
                stateMachine.ChangeState(enemyStateMachine.idleState);
            }
        }
    }

    public override void UpdatePhysics() {
        holderPosition = enemyStateMachine.transform.position;
        playerPosition = enemyStateMachine.playerGameObject.transform.position;

        if(path != null && path.Count() <= 0)
        {
            followingPath = false;
            enemyStateMachine.isFleeing = false;
        }

        if(!hasAskedPath && !followingPath)
        {
            hasAskedPath = true;

            Vector3 fleePoint = holderPosition + (holderPosition - playerPosition).normalized * enemyStateMachine.fleeDistance;

            Debug.Log("enemy tryied to find a path to runaway");
            enemyStateMachine.pathRequestManager.RequestPath(holderPosition, fleePoint, OnPathFound); 
        }
        else if(followingPath)
        {
            FollowPath();
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if(pathSuccessful && enemyStateMachine.currentState == this)
        {
            Debug.Log("it succeeded");
            targetIndex = 0;
            hasAskedPath = false;
            followingPath = true;
			path = newPath;
		}
        else
        {
            Debug.Log("it failed");
            enemyStateMachine.isFleeing = false;
        }
	}

    public void FollowPath() 
    {
        //enemyStateMachine.animator.SetBool("isMoving", true);
        // Debug.Log("tamanho do caminho: " + path.Count());
		Vector3 currentWaypoint = path[targetIndex];
        
		if(Vector3.Distance(holderPosition, currentWaypoint) <= 0.1) 
        {
            //Debug.Log("AM I  BECOMING FUICKIN CRAZY??? tagetIndex = " + targetIndex);
			targetIndex ++;
			if(targetIndex >= path.Count()) 
            {
                Debug.Log("end of flee");
                followingPath = false;
                enemyStateMachine.isFleeing = false;
                return;
			}
			// currentWaypoint = path[targetIndex];
		}
        
        enemyStateMachine.characterOrientation.ChangeOrientation(currentWaypoint);

        Vector3 movementDirection = currentWaypoint - holderPosition;
        enemyStateMachine.rigidBody.velocity = movementDirection.normalized * enemyStateMachine.movementSpeed;
	}

    public override void Exit() 
    {
        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        enemyStateMachine.canFlee = false;
        enemyStateMachine.StartCoroutine(enemyStateMachine.Cooldown("flee"));
        
        followingPath = false;
        path = null;
    }
}
