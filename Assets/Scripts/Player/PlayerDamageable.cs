using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : Damageable
{
    [SerializeField] float maxHealth;
    public bool damageable;
    [HideInInspector] private PlayerStateMachine stateMachine;

    // public EnemyDamageable() : base("Move", stateMachine) {
    //     moveState = MovementState.WALKING;
    // }

    public void Start()
    {
        stateMachine = GetComponent<PlayerStateMachine>();
        currentHealth = maxHealth;
        damageable = true;
    }

    public override void Damage(float damageAmount, Vector3 attackerPosition)
    {
        if(damageable)
        {
            Vector3 knockbackVector = (attackerPosition - transform.position).normalized * -1;

            currentHealth -= damageAmount;
            if(currentHealth <= 0)
            {
                stateMachine.ChangeState(stateMachine.deadState);
            }
            else
            {
                stateMachine.knockbackVector = knockbackVector;
                stateMachine.ChangeState(stateMachine.damageState);
            }
        }
    }
}