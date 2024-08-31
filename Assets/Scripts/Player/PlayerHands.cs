using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    [SerializeField] public Transform attacksParentGameObject;
    [SerializeField] public Animator handsAnimator;

    PlayerStateMachine playerStateMachine;

    [SerializeField] List<GameObject> attacks;
    public Attack actualAttack;

    void Awake()
    {
        playerStateMachine = GetComponentInParent<PlayerStateMachine>(); 
        attacksParentGameObject = GameObject.Find("Attacks").transform;
        handsAnimator = GetComponent<Animator>(); 
    }

    public void Attack(Vector3 attackDirection)
    {
        if(attacks.Count <= 0)
        {
            Debug.Log("There isn't any attacks settled");
        }
        else if(attacks[0].GetComponent<Attack>().isProjectile)
        {
            GameObject projectile = Instantiate(attacks[0], transform.position, Quaternion.identity, attacksParentGameObject);
            
            actualAttack = projectile.GetComponent<Attack>();
            projectile.GetComponent<Rigidbody2D>().AddForce(attackDirection * actualAttack.fireForce, ForceMode2D.Impulse);
            Destroy(projectile, actualAttack.projectileDuration);
        }
        else
        {
            attacks[0].SetActive(true);
            actualAttack = attacks[0].GetComponent<Attack>();
        }
        
        handsAnimator.SetTrigger("playerAttack");
        actualAttack.ExecuteAttack();
    }

    public void Attack(Vector3 attackDirection, int attackIndex)
    {
        if(attacks.Count <= 0)
        {
            Debug.Log("There isn't any attacks settled");
        }
        else if(attacks[attackIndex].GetComponent<Attack>().isProjectile)
        {
            GameObject projectile = Instantiate(attacks[attackIndex], transform.position, Quaternion.identity, attacksParentGameObject);
            
            actualAttack = projectile.GetComponent<Attack>();
            projectile.GetComponent<Rigidbody2D>().AddForce(attackDirection * actualAttack.fireForce, ForceMode2D.Impulse);
            Destroy(projectile, 2f);
        }
        else
        {
            attacks[attackIndex].SetActive(true);
            actualAttack = attacks[attackIndex].GetComponent<Attack>();
        }
        
        handsAnimator.SetTrigger("playerAttack");
        actualAttack.ExecuteAttack();
    }

    public void AttackEnd()
    {
        if(!actualAttack.isProjectile)
        {
            actualAttack.StopAttack();
        }
    
        playerStateMachine.attacked = false;
    }
}

