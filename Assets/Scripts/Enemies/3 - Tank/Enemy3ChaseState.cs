using System.Linq;
using UnityEngine;

public class Enemy3ChaseState : BaseState
{
    Enemy3StateMachine enemyStateMachine;
    
    public Vector3 holderPosition;
    public Vector3 playerPosition;
    public Vector3 lastPlayerPosition;
    public bool hasAskedPath;
    public bool followingPath;

    int targetIndex;
    Vector3[] path;

    public Enemy3ChaseState(Enemy3StateMachine stateMachine) : base("Chase", stateMachine) {
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

    public override void UpdatePhysics() 
    {
        if(enemyStateMachine.canRun && !enemyStateMachine.runIsLoading && Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfEngage)
        {
            enemyStateMachine.runIsLoading = true;
            enemyStateMachine.loadRun = enemyStateMachine.StartCoroutine(enemyStateMachine.LoadRun());
        }
        else if(enemyStateMachine.runIsLoading && Vector3.Distance(holderPosition, playerPosition) > enemyStateMachine.rangeOfEngage)
        {
            enemyStateMachine.runIsLoading = false;
            enemyStateMachine.StopCoroutine(enemyStateMachine.loadRun);
        }

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
            Debug.Log("Não achou uma solução de caminho");
            stateMachine.ChangeState(enemyStateMachine.idleState);
        }
	}

    public void FollowPath() 
    {
		Vector3 currentWaypoint = path[targetIndex];
        
		if (Vector3.Distance(holderPosition, currentWaypoint) <= 0.1) 
        {
			targetIndex ++;
			if(targetIndex >= path.Count()) 
            {
                // Debug.Log("HEHEHEHE");
                followingPath = false;
                return;
			}
		}
        
        enemyStateMachine.characterOrientation.ChangeOrientation(currentWaypoint);

        Vector3 movementDirection = currentWaypoint - holderPosition;
        enemyStateMachine.rigidBody.velocity = movementDirection.normalized * enemyStateMachine.movementSpeed;
	}

    public override void Exit() 
    {
        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        followingPath = false;
        path = null;

        if(enemyStateMachine.runIsLoading)
        {
            enemyStateMachine.runIsLoading = false;
            enemyStateMachine.StopCoroutine(enemyStateMachine.loadRun);
        }
    }
}