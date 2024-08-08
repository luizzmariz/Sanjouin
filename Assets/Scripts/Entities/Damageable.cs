using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour 
{
    public float currentHealth;
    public virtual void Damage(float damageAmount, Vector3 knockbackVector) {}
}