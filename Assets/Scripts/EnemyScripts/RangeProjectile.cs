using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class RangeProjectile : MonoBehaviour
{
    public int damage; // 데미지
    public float projectileSpeed = 2f; // 투사체 속도
    public float lifeTime = 3f; // 생명주기
    public float angle = 45f; // 발사각
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject,lifeTime);
    }

    public void Initialize(Vector3 dir, int dmg)
    {
        damage = dmg;
        if (rb != null) 
        {
            rb.velocity = dir * projectileSpeed; // 날아가기

            rb.angularVelocity = new Vector3(0f, 0f, 10f); // 초당 회전 속도 (라디안 단위)
        }
    } // Initialize ed

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            Player player = hit.gameObject.GetComponent<Player>();
            GameManager GM = GetComponent<GameManager>();

            if (player != null)
            {
                Destroy(gameObject);
                GameManager.Instance.TakeDamage(damage);
                Debug.Log("플레이어 데미지! 남은체력 : " + GameManager.Instance.HP);
                player.animator.Play("Player_damage");
            }
        } else {Destroy(gameObject);}
    } // OTE ed
} // RangeProjectile ED
