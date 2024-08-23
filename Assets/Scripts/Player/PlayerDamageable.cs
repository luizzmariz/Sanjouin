using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : Damageable
{
    [SerializeField] float maxHealth;
    public bool damageable;
    [HideInInspector] private PlayerStateMachine stateMachine;
    public PlayerHealthBar playerHealthBar;

    public void Awake()
    {
        if(playerHealthBar == null)
        {
            playerHealthBar = GameObject.Find("Canvas").transform.Find("HealthBar").GetComponent<PlayerHealthBar>();
        }
    }

    public void Start()
    {
        stateMachine = GetComponent<PlayerStateMachine>();
        currentHealth = maxHealth;
        playerHealthBar.CheckHearths(currentHealth, maxHealth);
        damageable = true;
    }

    public override void Damage(float damageAmount, Vector3 attackerPosition)
    {
        if(damageable)
        {
            Vector3 knockbackVector = (transform.position - attackerPosition).normalized;

            currentHealth -= damageAmount;

            playerHealthBar.CheckHearths(currentHealth, maxHealth);

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

    public void Heal(float healAmount)
    {
        while(healAmount > 0)
        {
            if(currentHealth < maxHealth)
            {
                currentHealth++;
            }
            healAmount--;
        }

        playerHealthBar.CheckHearths(currentHealth, maxHealth);
    }
}