using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHands : MonoBehaviour
{
    public enum WeaponType
    {
        Melee,
        Bullet
    }

    public WeaponType weaponType;

    BaseEnemyStateMachine enemyStateMachine;
    [SerializeField] private int damageAmount;

    // [SerializeField] List<GameObject> attacks;

    void Awake()
    {
        enemyStateMachine = GetComponentInParent<BaseEnemyStateMachine>(); 
    }

    // public void Attack(float attackType, int)
    // {
    //     // if(attackIndex == 0)
    //     // {
    //     //     transform.Get
    //     // }
        
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerDamageable>())
        {
            if(weaponType == WeaponType.Bullet)
            {
                collision.GetComponent<PlayerDamageable>().Damage(damageAmount, transform.position - (Vector3)GetComponent<Rigidbody2D>().velocity * 1.5f);
                Destroy(gameObject);
            }
            else
            {
                collision.GetComponent<PlayerDamageable>().Damage(damageAmount, transform.position);
            }
        }
    }

    public void AttackEnd()
    {
        // if(weaponType == WeaponType.Melee)
        // {
            enemyStateMachine.isAttacking = false;
        // }
    }
}
