using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Attack
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(!usedColliders.Contains(collider))
            {   
                if(collider.GetComponent<PlayerDamageable>())
                {
                    DealDamage(collider);
                }
                else if(collider.gameObject.layer == LayerMask.NameToLayer("Collision") || 
                (collider.gameObject.layer == LayerMask.NameToLayer("Damageable") &&
                !collider.gameObject.GetComponent<EnemyDamageable>()))
                {
                    if(isProjectile)
                    {
                        Destroy(gameObject);
                    }
                }
                usedColliders.Add(collider);
            }
    }

    protected override void DealDamage(Collider2D collider) {
        if(isProjectile)
        {
            if(collider.GetComponent<PlayerDamageable>().damageable)
            {
                Destroy(gameObject);
            }
            collider.GetComponent<PlayerDamageable>().Damage(damageAmount, transform.position - (Vector3)GetComponent<Rigidbody2D>().velocity * 1.5f);
        }
        else
        {
            collider.GetComponent<PlayerDamageable>().Damage(damageAmount, transform.position);
        }
    }

    public override void StopAttack()
    {
        base.StopAttack();
    }
}
