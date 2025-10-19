using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAtkHitbox : MonoBehaviour
{
    public int damage = 1;
    

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            Player player = hit.gameObject.GetComponent<Player>();
            GameManager GM = GetComponent<GameManager>();   

            if (player != null)
            {
                GameManager.Instance.TakeDamage(damage);
                Debug.Log("�÷��̾� ������! ����ü�� : " + GameManager.Instance.HP);
                player.animator.Play("Player_damage");
            }
        }
    } // OTE ed

} // EAH ed
