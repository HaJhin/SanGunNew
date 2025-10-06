using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPIcon : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();        
    }

    public void PlayDamage()
    {
        animator.SetTrigger("Damage");
    }

    public void PlayRecover()
    {
        animator.SetTrigger("Recover");
    }
}
