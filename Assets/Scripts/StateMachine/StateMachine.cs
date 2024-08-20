using UnityEngine;

public class StateMachine : MonoBehaviour {
    public BaseState currentState;

    void Start() 
    {
        currentState = GetInitialState();
        if (currentState != null)
            currentState.Enter();
    }

    protected virtual void Update() 
    {
        if (currentState != null)
            currentState.UpdateLogic();
    }

    void LateUpdate() 
    {
        if (currentState != null)
            currentState.UpdatePhysics();
    }

    public void ChangeState(BaseState newState) 
    {
        if(newState != currentState)
        {
            currentState.Exit();
        
            currentState = newState;
            currentState.Enter();
        }
    }

    protected virtual BaseState GetInitialState() 
    {
        return null;
    }
}