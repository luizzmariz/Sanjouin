using System.Linq;
using UnityEngine;

public class CreatureFleeState : BaseState
{
    SimpleCreatureStateMachine creatureStateMachine;

    public Vector3 holderPosition;
    public Vector3 playerPosition;
    public bool hasAskedPath;
    public bool followingPath;

    int targetIndex;
    public Vector3[] path;

    public CreatureFleeState(SimpleCreatureStateMachine stateMachine) : base("Flee", stateMachine) {
        creatureStateMachine = stateMachine;
    }

    public override void Enter() {
        creatureStateMachine.isFleeing = true;
    }

    public override void UpdateLogic() {
        holderPosition = creatureStateMachine.transform.position;
        playerPosition = creatureStateMachine.playerGameObject.transform.position;
        
        if(Vector3.Distance(holderPosition, playerPosition) > creatureStateMachine.fleeDistance)
        {
            stateMachine.ChangeState(creatureStateMachine.idleState);
        }
        else if(!creatureStateMachine.isFleeing)
        {
            stateMachine.ChangeState(creatureStateMachine.idleState);
        }
    }

    public override void UpdatePhysics() {
        holderPosition = creatureStateMachine.transform.position;
        playerPosition = creatureStateMachine.playerGameObject.transform.position;

        if(path != null && path.Count() <= 0)
        {
            followingPath = false;
            creatureStateMachine.isFleeing = false;
        }

        if(!hasAskedPath && !followingPath)
        {
            hasAskedPath = true;

            Vector3 fleePoint = holderPosition + (holderPosition - playerPosition).normalized * creatureStateMachine.fleeDistance;

            creatureStateMachine.pathRequestManager.RequestPath(holderPosition, fleePoint, OnPathFound, creatureStateMachine.gameObject); 
        }
        else if(followingPath)
        {
            FollowPath();
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if(pathSuccessful && creatureStateMachine.currentState == this)
        {
            targetIndex = 0;
            hasAskedPath = false;
            followingPath = true;
			path = newPath;
		}
        else
        {
            Debug.Log("it failed");
            creatureStateMachine.isFleeing = false;
        }
	}

    public void FollowPath() 
    {
		Vector3 currentWaypoint = path[targetIndex];
        
		if(Vector3.Distance(holderPosition, currentWaypoint) <= 0.1) 
        {
			targetIndex ++;
			if(targetIndex >= path.Count()) 
            {
                Debug.Log("end of flee");
                followingPath = false;
                creatureStateMachine.isFleeing = false;
                return;
			}
		}
        
        creatureStateMachine.characterOrientation.ChangeOrientation(currentWaypoint);

        Vector3 movementDirection = currentWaypoint - holderPosition;
        creatureStateMachine.rigidBody.velocity = movementDirection.normalized * creatureStateMachine.movementSpeed;
	}

    public override void Exit() 
    {
        creatureStateMachine.rigidBody.velocity = Vector3.zero;
        creatureStateMachine.canFlee = false;
        creatureStateMachine.StartCoroutine(creatureStateMachine.Cooldown("flee"));
        
        followingPath = false;
        path = null;
    }
}
