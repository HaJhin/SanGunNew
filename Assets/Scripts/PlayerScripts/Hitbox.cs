using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider hit)
    {
        EnemyDamage target = hit.GetComponent<EnemyDamage>();
        if (target != null )
        {
            target.TakeDamage(GameManager.Instance.atk);
        } // if ed
    } // OTE ed
} // class ed 
