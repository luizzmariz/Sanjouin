using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy1ChaseState : BaseState
{
    Enemy1StateMachine enemyStateMachine;
    
    public Vector3 holderPosition;
    public Vector3 playerPosition;
    public Vector3 lastPlayerPosition;
    public bool hasAskedPath;
    public bool followingPath;

    int targetIndex;
    Vector3[] path;

    public Enemy1ChaseState(Enemy1StateMachine stateMachine) : base("Chase", stateMachine) {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {
        enemyStateMachine.enemyDamageable.damageable = true;
        hasAskedPath = false;
        followingPath = false;
    }

    public override void UpdateLogic() {
        if(enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().currentState == enemyStateMachine.playerGameObject.GetComponent<PlayerStateMachine>().deadState)
        {
            stateMachine.ChangeState(enemyStateMachine.idleState);
        }

        holderPosition = enemyStateMachine.transform.position;
        playerPosition = enemyStateMachine.playerGameObject.transform.position;
        
        if(Vector3.Distance(holderPosition, playerPosition) > enemyStateMachine.rangeOfView)
        {
            stateMachine.ChangeState(enemyStateMachine.idleState);
        }
        else if(Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfAttack)
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
    }

    //This function runs at LateUpdate()
    public override void UpdatePhysics() {
        //base.UpdatePhysics();

        holderPosition = enemyStateMachine.transform.position;
        playerPosition = enemyStateMachine.playerGameObject.transform.position;

        if(path != null && path.Count() <= 0)
        {
            followingPath = false;
        }

        if(Vector3.Distance(holderPosition, playerPosition) < enemyStateMachine.rangeOfView * 0.8f)
        {
            if(!hasAskedPath && !followingPath)
            {
                hasAskedPath = true;
                // Debug.Log("Pedinddo caminho, hasAskedPath = " + hasAskedPath + ", followingPath = " + followingPath);
                lastPlayerPosition = playerPosition;
                enemyStateMachine.pathRequestManager.RequestPath(holderPosition, playerPosition, OnPathFound, enemyStateMachine.gameObject); 
            }
            else if(followingPath)
            {
                if(Vector3.Distance(playerPosition, lastPlayerPosition) > 1.25f)
                {
                    followingPath = false;
                    hasAskedPath = true;
                    lastPlayerPosition = playerPosition;
                    enemyStateMachine.pathRequestManager.RequestPath(holderPosition, playerPosition, OnPathFound, enemyStateMachine.gameObject); 
                }
                else
                {
                    FollowPath();
                }
                // Debug.Log("Seguindo caminho, hasAskedPath = " + hasAskedPath + ", followingPath = " + followingPath);
            }
        }
        else
        {
            enemyStateMachine.rigidBody.velocity = (playerPosition - holderPosition).normalized * enemyStateMachine.movementSpeed;
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

	// public void OnDrawGizmos() 
    // {
	// 	if (path != null) {
	// 		for (int i = targetIndex; i < path.Length; i ++) {
	// 			Gizmos.color = Color.black;
	// 			Gizmos.DrawCube(path[i], Vector3.one);

	// 			if (i == targetIndex) {
	// 				Gizmos.DrawLine(holderPosition, path[i]);
	// 			}
	// 			else {
	// 				Gizmos.DrawLine(path[i-1],path[i]);
	// 			}
	// 		}
	// 	}
	// }

    public override void Exit() 
    {
        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        followingPath = false;
        path = null;
    }
}