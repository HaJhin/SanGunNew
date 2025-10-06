using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 5;

    private void OnTriggerEnter(Collider hit)
    {
        EnemyDamage target = hit.GetComponent<EnemyDamage>();
        if (target != null )
        {
            target.TakeDamage(damage);
        } // if ed
    } // OTE ed
} // class ed 
