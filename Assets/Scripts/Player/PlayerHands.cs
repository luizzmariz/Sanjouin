using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{

    public enum WeaponType{Melee,Bullet}
    public WeaponType weaponType;
    public LayerMask layerMask;
    [SerializeField] private int damageAmount;

    public bool isAttacking;
    List<Collider2D> colliders;
    List<Collider2D> usedColliders;
    ContactFilter2D contactFilter2D;

    void Awake()
    {
        colliders = new List<Collider2D>();
        usedColliders = new List<Collider2D>();
        contactFilter2D = new ContactFilter2D
        {
            layerMask = this.layerMask,
            useLayerMask = true
        };

        isAttacking = false;
    }

    public virtual void ExecuteAttack()
    {
        usedColliders = new List<Collider2D>();
        isAttacking = true;
        Debug.Log("ataque");
    }

    // void Update()
    // {
    //     if(isAttacking && GetComponent<Collider2D>().OverlapCollider(contactFilter2D, colliders) > 0)
    //     {
    //         foreach(Collider2D collider in colliders)
    //         {
    //             if(!usedColliders.Contains(collider))
    //             {
    //                 if(collider.GetComponent<EnemyDamageable>())
    //                 {
    //                     Debug.Log("chjegou aquimfj");
    //                     DealDamage(collider);
    //                     Destroy(gameObject);
    //                 }
    //                 else if(!collider.GetComponent<PlayerDamageable>())
    //                 {
    //                     Destroy(gameObject);
    //                 }
    //                 usedColliders.Add(collider);
    //             }
    //         }
    //     }
    // }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(isAttacking && !usedColliders.Contains(col))
                {
                    if(col.GetComponent<EnemyDamageable>())
                    {
                        DealDamage(col);
                        if(weaponType == WeaponType.Bullet)
                        {
                            Destroy(gameObject);
                        }
                        
                    }
                    else if(!col.GetComponent<PlayerDamageable>())
                    {
                        if(weaponType == WeaponType.Bullet)
                        {
                            Destroy(gameObject);
                        }
                    }
                    usedColliders.Add(col);
                }
    }

    // void OnTriggerExit2D(Collider2D col)
    // {
    //                 if(weaponType == WeaponType.Bullet && col.GetComponent<PlayerDamageable>())
    //                 {
                        
                        
    //                 }
    // }

    protected virtual void DealDamage(Collider2D collider)
    {
        if(weaponType ==  WeaponType.Bullet)
        {
            collider.GetComponent<EnemyDamageable>().Damage(damageAmount, transform.position - (Vector3)GetComponent<Rigidbody2D>().velocity * 1.5f);
        }
        else
        {
            collider.GetComponent<EnemyDamageable>().Damage(damageAmount, transform.position);
        }
    }

    public virtual void StopAttack()
    {
        isAttacking = false;
    }
}

