using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHands : MonoBehaviour
{
    EnemyStateMachine enemyStateMachine;
    Enemy2StateMachine enemy2StateMachine;
    [SerializeField] private int damageAmount;
    // public Collider2D attack1collider;

    void Awake()
    {
        enemyStateMachine = GetComponentInParent<EnemyStateMachine>();
        if(enemyStateMachine == null)
        {
            enemy2StateMachine = GetComponentInParent<Enemy2StateMachine>();
        }   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerDamageable>())
        {
            collision.GetComponent<PlayerDamageable>().Damage(damageAmount, transform.position);
        }
    }

    public void AttackEnd()
    {
        if(enemyStateMachine == null)
        {
            enemy2StateMachine.isAttacking = false;
        }
        else
        {
            enemyStateMachine.isAttacking = false;
        }
    }
}
