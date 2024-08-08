using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHands : MonoBehaviour
{
    EnemyStateMachine enemyStateMachine;
    [SerializeField] private int damageAmount;
    // public Collider2D attack1collider;

    void Awake()
    {
        enemyStateMachine = GetComponentInParent<EnemyStateMachine>();
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
        enemyStateMachine.isAttacking = false;
    }
}
