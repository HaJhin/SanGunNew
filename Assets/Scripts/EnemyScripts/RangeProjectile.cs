using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class RangeProjectile : MonoBehaviour
{
    public int damage; // ������
    public float projectileSpeed = 2f; // ����ü �ӵ�
    public float lifeTime = 3f; // �����ֱ�
    public float angle = 45f; // �߻簢
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
            rb.velocity = dir * projectileSpeed; // ���ư���

            rb.angularVelocity = new Vector3(0f, 0f, 10f); // �ʴ� ȸ�� �ӵ� (���� ����)
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
                Debug.Log("�÷��̾� ������! ����ü�� : " + GameManager.Instance.HP);
                player.animator.Play("Player_damage");
            }
        } else {Destroy(gameObject);}
    } // OTE ed
} // RangeProjectile ED
