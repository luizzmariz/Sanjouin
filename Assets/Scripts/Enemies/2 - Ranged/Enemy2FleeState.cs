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
    Vector3[] path;

    public Enemy2FleeState(Enemy2StateMachine stateMachine) : base("Flee", stateMachine) {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {
        enemyStateMachine.canMove = true;
        enemyStateMachine.enemyDamageable.damageable = true;
    }

    public override void UpdateLogic() {
        holderPosition = enemyStateMachine.transform.position;
        playerPosition = enemyStateMachine.playerGameObject.transform.position;

        if(Vector3.Distance(holderPosition, playerPosition) > enemyStateMachine.rangeOfDanger)
        {
            enemyStateMachine.rigidBody.velocity = Vector3.zero;
            
            // followingPath = false;
            // path = null;

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
        }

        if(!hasAskedPath && !followingPath)
        {
            hasAskedPath = true;
            enemyStateMachine.pathRequestManager.RequestPath(holderPosition, playerPosition, OnPathFound); 
        }
        else if(followingPath)
        {
            FollowPath();
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful && enemyStateMachine.currentState == this)
        {
            // Debug.Log("Caminho chegou, hasAskedPath = " + hasAskedPath + ", followingPath = " + followingPath);
            // for(int i = 0; i < newPath.Length; i++)
            // {
            //     //newPath[i].y = 5;
            //     //Debug.Log("wayPoint " + i + " is: " + newPath[i]);
            // }
            targetIndex = 0;
            hasAskedPath = false;
            followingPath = true;
			path = newPath;
		}
        else
        {
            stateMachine.ChangeState(enemyStateMachine.idleState);
        }
	}

    public void FollowPath() 
    {
        //enemyStateMachine.animator.SetBool("isMoving", true);
        // Debug.Log("tamanho do caminho: " + path.Count());
		Vector3 currentWaypoint = path[targetIndex];
        
		if (Vector3.Distance(holderPosition, currentWaypoint) <= 0.1) 
        {
            //Debug.Log("AM I  BECOMING FUICKIN CRAZY??? tagetIndex = " + targetIndex);
			targetIndex ++;
			if(targetIndex >= path.Count()) 
            {
                // Debug.Log("HEHEHEHE");
                followingPath = false;
                return;
			}
			// currentWaypoint = path[targetIndex];
		}
        
        enemyStateMachine.characterOrientation.ChangeOrientation(currentWaypoint);

        Vector3 movementDirection = currentWaypoint - holderPosition;
        enemyStateMachine.rigidBody.velocity = movementDirection.normalized * enemyStateMachine.movementSpeed;
	}
}
