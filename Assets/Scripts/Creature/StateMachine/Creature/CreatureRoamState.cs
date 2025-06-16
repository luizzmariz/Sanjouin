using System.Linq;
using UnityEngine;

public class CreatureRoamState : BaseState
{
    SimpleCreatureStateMachine creatureStateMachine;

    public Vector3 holderPosition;
    public Vector3 playerPosition;
    public bool hasAskedPath;
    public bool followingPath;

    int targetIndex;
    public Vector3[] path;

    public CreatureRoamState(SimpleCreatureStateMachine stateMachine) : base("Roam", stateMachine) {
        creatureStateMachine = stateMachine;
    }

    public override void Enter() {
        creatureStateMachine.isRoaming = true;
    }

    public override void UpdateLogic() {
        holderPosition = creatureStateMachine.transform.position;
        playerPosition = creatureStateMachine.playerGameObject.transform.position;
        
        if(Vector3.Distance(holderPosition, playerPosition) < creatureStateMachine.rangeOfDanger)
        {
            stateMachine.ChangeState(creatureStateMachine.fleeState);
        }
        else if(!creatureStateMachine.isRoaming)
        {
            stateMachine.ChangeState(creatureStateMachine.idleState);
        }
    }

    public override void UpdatePhysics() {
        holderPosition = creatureStateMachine.transform.position;

        if(path != null && path.Count() <= 0)
        {
            followingPath = false;
            creatureStateMachine.isRoaming = false;
        }

        if(!hasAskedPath && !followingPath)
        {
            hasAskedPath = true;

            Vector3 roamPoint = SetRoamDirection(); 

            creatureStateMachine.pathRequestManager.RequestPath(holderPosition, roamPoint, OnPathFound, creatureStateMachine.gameObject); 
        }
        else if(followingPath)
        {
            FollowPath();
        }
    }

    public Vector3 SetRoamDirection()
    {
        Vector3 roamDirection = new Vector2(Random.Range(-creatureStateMachine.maxRoamDistance, creatureStateMachine.maxRoamDistance),
        Random.Range(-creatureStateMachine.maxRoamDistance, creatureStateMachine.maxRoamDistance));
        float roamDistance = Random.Range(0.5f, creatureStateMachine.maxRoamDistance);

        Vector3 roamPoint = holderPosition + roamDirection.normalized * roamDistance;

        // Debug.Log(creatureStateMachine.name + " position is: " + holderPosition + ". roamPoint is: " + roamPoint
        //  + " (roamDirection: " + roamDirection + ", roamDistance: " + roamDistance + ").");

        return roamPoint; 
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful && creatureStateMachine.currentState == this)
        {
            targetIndex = 0;
            hasAskedPath = false;
            followingPath = true;
            path = newPath;
        }
        else
        {
            hasAskedPath = false;
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
                followingPath = false;
                creatureStateMachine.isRoaming = false;
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
        creatureStateMachine.canRoam = false;
        creatureStateMachine.StartCoroutine(creatureStateMachine.Cooldown("roam"));
        
        followingPath = false;
        path = null;
    }
}
