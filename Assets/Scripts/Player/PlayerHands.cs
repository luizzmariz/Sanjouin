using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{

    public enum WeaponType{Melee,Bullet}
    public WeaponType weaponType;
    [SerializeField] private int damageAmount;

    void Awake()
    {
         
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyDamageable>())
        {
            collision.GetComponent<EnemyDamageable>().Damage(damageAmount, transform.position);
            if(weaponType == WeaponType.Bullet){
                Destroy(gameObject);
            }
        }
    }

    public void AttackEnd()
    {
        
    }
}

