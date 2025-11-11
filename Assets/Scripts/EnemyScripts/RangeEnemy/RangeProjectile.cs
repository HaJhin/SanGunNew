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

    public GameObject prjEffect;

    public AudioSource audioSource;
    public AudioClip boomClip;

    public Collider colider;
    public SpriteRenderer sr;
    
    private void Awake()
    {
        colider = GetComponent<Collider>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
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
                PrjEffect();
                audioSource.PlayOneShot(boomClip);
                Color color = sr.color; color.a = 0f; sr.color = color;
                colider.enabled = false;
                Destroy(gameObject,0.5f);
                GameManager.Instance.TakeDamage(damage);
                Debug.Log("플레이어 데미지! 남은체력 : " + GameManager.Instance.HP);
                player.animator.Play("Player_damage");
            }
        } else 
        {
            PrjEffect();
            audioSource.PlayOneShot(boomClip);
            Color color = sr.color; color.a = 0f; sr.color = color;
            colider.enabled = false;
            Destroy(gameObject,0.5f);
        }
    } // OTE ed

    private void PrjEffect()
    {
        if (prjEffect != null)
        {
            GameObject vfx = Instantiate(prjEffect, transform.position, Quaternion.identity);
            Destroy(vfx, 1f);
        }
    } // PrjEffect ed

} // RangeProjectile ED
