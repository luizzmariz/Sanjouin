using System.Linq;
using UnityEngine;

public class Enemy4FleeState : BaseState
{
    Enemy4StateMachine enemyStateMachine;

    public Vector3 holderPosition;
    public Vector3 playerPosition;
    public bool hasAskedPath;
    public bool followingPath;

    int targetIndex;
    public Vector3[] path;

    bool isFleeing;

    public Enemy4FleeState(Enemy4StateMachine stateMachine) : base("Flee", stateMachine) {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {
        isFleeing = true;
    }

    public override void UpdateLogic() {
        holderPosition = enemyStateMachine.transform.position;
        playerPosition = enemyStateMachine.playerGameObject.transform.position;
        
        if(Vector3.Distance(holderPosition, playerPosition) > enemyStateMachine.rangeOfDig)
        {
            stateMachine.ChangeState(enemyStateMachine.idleState);
        }
        else if(!isFleeing)
        {
            stateMachine.ChangeState(enemyStateMachine.idleState);
        }
    }

    public override void UpdatePhysics() {
        if(path != null && path.Count() <= 0)
        {
            followingPath = false;
            isFleeing = false;
        }

        if(!hasAskedPath && !followingPath)
        {
            holderPosition = enemyStateMachine.transform.position;
            playerPosition = enemyStateMachine.playerGameObject.transform.position;

            hasAskedPath = true;

            Vector3 fleePoint = holderPosition + (holderPosition - playerPosition).normalized * enemyStateMachine.fleeDistance;

            enemyStateMachine.pathRequestManager.RequestPath(holderPosition, fleePoint, OnPathFound, enemyStateMachine.gameObject); 
        }
        else if(followingPath)
        {
            FollowPath();
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if(pathSuccessful && enemyStateMachine.currentState == this)
        {
            targetIndex = 0;
            hasAskedPath = false;
            followingPath = true;
			path = newPath;
		}
        else
        {
            targetIndex = 0;
            hasAskedPath = false;
            followingPath = true;
            path = new Vector3[]{holderPosition + (holderPosition - playerPosition).normalized * enemyStateMachine.fleeDistance};
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
                followingPath = false;
                isFleeing = false;
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
        enemyStateMachine.StartCoroutine(enemyStateMachine.Cooldown("flee"));
        
        followingPath = false;
        path = null;
    }
}
