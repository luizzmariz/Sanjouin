using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public LayerMask layerMask;
    public bool isProjectile;
    public int damageAmount;
    [SerializeField] protected bool isAttacking;
    public Animator animator;
    public float fireForce;
    public float projectileDuration;

    protected List<Collider2D> colliders;
    protected List<Collider2D> usedColliders;
    protected ContactFilter2D contactFilter2D;

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
        
    }

    protected virtual void DealDamage(Collider2D collider)
    {

    }

    public virtual void StopAttack()
    {
        isAttacking = false;
        gameObject.SetActive(false);
    }
}
