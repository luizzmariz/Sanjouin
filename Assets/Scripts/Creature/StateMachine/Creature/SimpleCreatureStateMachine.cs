using System.Collections;
using UnityEngine;

public class SimpleCreatureStateMachine : BaseCreatureStateMachine
{
    [Header("States")]
    [HideInInspector] public CreatureIdleState idleState;
    [HideInInspector] public CreatureChaseState chaseState;
    [HideInInspector] public CreatureFleeState fleeState;
    [HideInInspector] public CreatureCaptureState damageState;
    [HideInInspector] public CreatureCapturedState deadState;


    [Header("Roam")]
    public bool canRoam;
    public bool isRoaming;
    public float roamCooldownTimer;
    public float roamDistance;
    
    [Header("Attributes")]
    [Range(0f, 25f)] public float rangeOfDanger;

    [Header("Flee")]
    public bool canFlee;
    public bool isFleeing;
    public float fleeCooldownTimer;
    public float fleeDistance;

    protected override void Awake() {
        base.Awake();

        idleState = new CreatureIdleState(this);
        chaseState = new CreatureChaseState(this);
        fleeState = new CreatureFleeState(this);
        damageState = new CreatureCaptureState(this);
        deadState = new CreatureCapturedState(this);

        canFlee = true;
    }

    protected override BaseState GetInitialState() {
        return idleState;
    }

    public override IEnumerator Cooldown(string ability)
    {
        switch(ability)
        {
            case "Roam":
            // Debug.Log("attack cooldown started");
            yield return new WaitForSeconds(roamCooldownTimer);
            // Debug.Log("attack cooldown ended");
            canRoam = true;
            break;

            case "flee":
            // Debug.Log("attack cooldown started");
            yield return new WaitForSeconds(fleeCooldownTimer);
            // Debug.Log("attack cooldown ended");
            canFlee = true;
            break;

            default:
            break;
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(250, 125, 200f, 150f));
        string content = currentState != null ? currentState.name : "(no current state)";
        GUILayout.Label($"<color='red'><size=40>{content}</size></color>");
        GUILayout.EndArea();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeOfView);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeOfAttack);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeOfDanger);
    }

    public override void TakeDamage(Vector3 knockbackVector) 
    {
        if(enemyDamageable.currentHealth <= 0)
        {
            ChangeState(deadState);
        }
        else
        {
            this.knockbackVector = knockbackVector;
            ChangeState(damageState);
        }
    }
}

