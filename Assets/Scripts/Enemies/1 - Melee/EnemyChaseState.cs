using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : BaseState
{
    EnemyStateMachine enemyStateMachine;
    
    public Vector3 holderPosition;
    public Vector3 playerPosition;
    public bool hasAskedPath;
    public bool followingPath;

    int targetIndex;
    Vector3[] path;

    public EnemyChaseState(EnemyStateMachine stateMachine) : base("Chase", stateMachine) {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {
        hasAskedPath = false;
        followingPath = false;
    }

    public override void UpdateLogic() {
        //base.UpdateLogic();
        
        holderPosition = enemyStateMachine.transform.position;
        playerPosition = enemyStateMachine.playerGameObject.transform.position;
        
        //ACHO Q PRECISO MELHORAR ESSAS CHECAGENS DE TROCA DE STATE, PQ TA MEIO ZOADO.


        if(Vector3.Distance(holderPosition, playerPosition) > enemyStateMachine.rangeOfView)
        {
            enemyStateMachine.rigidBody.velocity = Vector3.zero;
            followingPath = false;
            path = null;

            stateMachine.ChangeState(enemyStateMachine.idleState);
        }
        else if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfAttack)
        {
            enemyStateMachine.rigidBody.velocity = Vector3.zero;
            //enemyStateMachine.animator.SetBool("isMoving", false);
            followingPath = false;
            path = null;

            if(enemyStateMachine.canAttack)
            {
                stateMachine.ChangeState(enemyStateMachine.attackState);
            }
            else
            {
                stateMachine.ChangeState(enemyStateMachine.idleState);
            }
        }
    }

    //This function runs at LateUpdate()
    public override void UpdatePhysics() {
        //base.UpdatePhysics();

        holderPosition = enemyStateMachine.transform.position;
        playerPosition = enemyStateMachine.playerGameObject.transform.position;

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
            Debug.Log("ok");
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
		Vector3 currentWaypoint = path[0];
        
		if (Vector3.Distance(holderPosition, currentWaypoint) <= 0.1) 
        {
            //Debug.Log("AM I  BECOMING FUICKIN CRAZY??? tagetIndex = " + targetIndex);
			targetIndex ++;
			if(targetIndex >= path.Length) 
            {
                followingPath = false;
                return;
			}
			currentWaypoint = path[targetIndex];
		}
        
        // enemyStateMachine.characterOrientation.ChangeOrientation(currentWaypoint); <------------------------

        Vector3 movementDirection = currentWaypoint - holderPosition;
        enemyStateMachine.rigidBody.velocity = movementDirection.normalized * enemyStateMachine.movementSpeed;
	}

	public void OnDrawGizmos() 
    {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex) {
					Gizmos.DrawLine(holderPosition, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}