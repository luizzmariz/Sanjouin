using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public LayerMask layerMask;
    public bool isProjectile;
    public int damageAmount;
    bool isAttacking;
    public Animator animator;
    public float fireForce;

    List<Collider2D> colliders;
    List<Collider2D> usedColliders;
    ContactFilter2D contactFilter2D;

    public void Awake()
    {
        colliders = new List<Collider2D>();
        usedColliders = new List<Collider2D>();
        contactFilter2D = new ContactFilter2D
        {
            layerMask = this.layerMask,
            useLayerMask = true
        };

        isAttacking = false;

        animator = GetComponent<Animator>(); 
    }

    public virtual void ExecuteAttack()
    {
        usedColliders = new List<Collider2D>();
        isAttacking = true;
    }

    protected virtual void Update()
    {
        if(isAttacking && GetComponent<Collider2D>().OverlapCollider(contactFilter2D, colliders) > 0)
        {
            foreach(Collider2D collider in colliders)
            {
                if(!usedColliders.Contains(collider))
                {
                    if(collider.GetComponent<PlayerDamageable>())
                    {
                        DealDamage(collider);
                    }
                    usedColliders.Add(collider);
                }
            }
        }
    }

    protected virtual void DealDamage(Collider2D collider)
    {
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

    public virtual void StopAttack()
    {
        isAttacking = false;
    }
}
