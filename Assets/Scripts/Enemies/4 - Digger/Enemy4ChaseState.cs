using System.Linq;
using UnityEngine;

public class Enemy4ChaseState : BaseState
{
    Enemy4StateMachine enemyStateMachine;
    
    public Vector3 holderPosition;
    public Vector3 playerPosition;
    public Vector3 lastPlayerPosition;
    public bool hasAskedPath;
    public bool followingPath;

    int targetIndex;
    Vector3[] path;

    public Enemy4ChaseState(Enemy4StateMachine stateMachine) : base("Chase", stateMachine) {
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
        else if(enemyStateMachine.canDig && Vector3.Distance(holderPosition, playerPosition) <= enemyStateMachine.rangeOfDig)
        {
            stateMachine.ChangeState(enemyStateMachine.digState);
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
            lastPlayerPosition = playerPosition;
            enemyStateMachine.pathRequestManager.RequestPath(holderPosition, playerPosition, OnPathFound); 
        }
        else if(followingPath)
        {
            if(Vector3.Distance(playerPosition, lastPlayerPosition) > 1.25f)
            {
                followingPath = false;
                hasAskedPath = true;
                lastPlayerPosition = playerPosition;
                enemyStateMachine.pathRequestManager.RequestPath(holderPosition, playerPosition, OnPathFound); 
            }
            else
            {
                FollowPath();
            }
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful && enemyStateMachine.currentState == this)
        {
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
    }
}