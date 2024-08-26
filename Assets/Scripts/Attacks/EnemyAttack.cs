using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Attack
{
    // protected override void Update() {
    //     if(isAttacking && GetComponent<Collider2D>().OverlapCollider(contactFilter2D, colliders) > 0)
    //     {
    //         foreach(Collider2D collider in colliders)
    //         {
    //             if(!usedColliders.Contains(collider))
    //             {
    //                 if(collider.GetComponent<PlayerDamageable>())
    //                 {
    //                     DealDamage(collider);
    //                 }
    //                 else if(!collider.GetComponent<EnemyDamageable>())
    //                 {
    //                     Debug.Log("hm");
    //                     if(isProjectile)
    //                     {
    //                         Destroy(gameObject);
    //                     }
    //                 }
    //                 usedColliders.Add(collider);
    //             }
    //         }
    //     }
    // }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(!usedColliders.Contains(collider))
            {   
                if(collider.GetComponent<PlayerDamageable>())
                {
                    DealDamage(collider);
                }
                else if(collider.gameObject.layer == layerMask)
                {
                    if(isProjectile)
                    {
                        Destroy(gameObject);
                    }
                }
                usedColliders.Add(collider);
            }
        
        // if(isAttacking && GetComponent<Collider2D>().IsTouchingLayers(layerMask))
        // {
        //     if(!usedColliders.Contains(collider))
        //     {
                
        //         if(collider.GetComponent<EnemyDamageable>())
        //         {
        //             DealDamage(collider);
        //         }
        //         else if(!collider.GetComponent<PlayerDamageable>())
        //         {
        //             if(isProjectile)
        //             {
        //                 Destroy(gameObject);
        //             }
        //         }
        //         usedColliders.Add(collider);
        //     }
        // }
    }

    protected override void DealDamage(Collider2D collider) {
        if(isProjectile)
        {
            collider.GetComponent<PlayerDamageable>().Damage(damageAmount, transform.position - (Vector3)GetComponent<Rigidbody2D>().velocity * 1.5f);
            if(collider.GetComponent<PlayerDamageable>().damageable)
            {
                Destroy(gameObject);
            }
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
