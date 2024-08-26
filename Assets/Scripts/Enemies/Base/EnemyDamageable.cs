using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageable : Damageable
{
    [SerializeField] public float maxHealth;
    public bool damageable = true;
    [HideInInspector] private BaseEnemyStateMachine stateMachine;

    public void Start()
    {
        stateMachine = transform.parent.GetComponent<BaseEnemyStateMachine>();
        currentHealth = maxHealth;
    }

    public override void Damage(float damageAmount, Vector3 attackerPosition)
    {
        if(damageable)
        {
            Debug.Log("chegou aqui");
            Vector3 knockbackVector = (attackerPosition - transform.position).normalized * -1;

            currentHealth -= damageAmount;

            stateMachine.TakeDamage(knockbackVector);
        }
    }
}