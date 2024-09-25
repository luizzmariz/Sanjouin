using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Attack
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(!usedColliders.Contains(collider))
            {   
                if(collider.GetComponent<EnemyDamageable>())
                {
                    DealDamage(collider);
                }
                else if(collider.gameObject.layer == LayerMask.NameToLayer("Collision") || 
                (collider.gameObject.layer == LayerMask.NameToLayer("Damageable") &&
                !(collider.gameObject.GetComponent<PlayerDamageable>())))
                {
                    if(isProjectile)
                    {
                        Destroy(gameObject);
                    }
                }
                usedColliders.Add(collider);
            }
    }

    protected override void DealDamage(Collider2D collider) 
    {
        if(isProjectile)
        {
            if(collider.GetComponent<EnemyDamageable>().damageable)
            {
                Destroy(gameObject);
            }
            collider.GetComponent<EnemyDamageable>().Damage(damageAmount, transform.position - (Vector3)GetComponent<Rigidbody2D>().velocity * 1.5f);
        }
        else
        {
            collider.GetComponent<EnemyDamageable>().Damage(damageAmount, transform.position);
        }
    }

    public override void StopAttack()
    {
        base.StopAttack();
    }
}
