using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gyuki : MonoBehaviour , EnemyDamage
{
    public enum BossState { Idle, Move, Atk, Die }
    public BossState currentState = BossState.Idle;

    public bool Dead = false;

    [Header("Stat")]
    public int HP = 100;

    [Header("Movement")]
    public float walkSpeed = 0.5f;
    public float detectionRange = 12f;
    public float atkRange = 3f;
    public float backstepDistance = 0.1f;

    [Header("Atk Settings")]
    public int atk1Damage = 1;
    public int atk2Damage = 2;
    private bool isAtk = false;
    private bool isCooling = false;

    [Header("References")]
    private Transform player;
    private Animator animator;
    public GameObject[] atkColliders;

    [Header("Effect")]
    public GameObject bleedingEffect;
    public GameObject dustPrefab;
    public Transform dustSpawnPoint;

    [Header("Sound")]
    private AudioSource audioSource;
    public AudioClip[] atkClips;
    public AudioClip idleClip;
    public AudioClip dieClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = idleClip;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        audioSource.Play();
        if (player == null)
        {
            GameObject go = GameObject.FindWithTag("Player");
            if (go != null) player = go.transform;
        }

    } // Start ed

    private void Update()
    {
        if (currentState != BossState.Die)
        {
            Debug.Log(currentState);
            CheckState();
            DoAction();
        }
    }

    private void CheckState()
    {
        if (HP <= 0)
        {
            currentState = BossState.Die;
           return;
        }

        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case BossState.Idle:
                if (distance < detectionRange)
                    currentState = BossState.Move;
                break;

            case BossState.Move:
                if (distance <= atkRange)
                    currentState = BossState.Atk;
                else if (distance > detectionRange)
                    currentState = BossState.Idle;
                break;

            case BossState.Atk:
                if (!isAtk && !isCooling)
                    currentState = BossState.Move;
                break;
        }
    } // CheckState ed

    private void DoAction()
    {
        switch (currentState)
        {
            case BossState.Idle:
                IdleAction();
                break;
            case BossState.Move:
                MoveAction();
                break;
            case BossState.Atk:
                AtkAction();
                break;
            case BossState.Die:
                DieAction();
                break;
        }
    } // DoAction ed

    private void IdleAction()
    {
        animator.SetBool("Move", false);
    } // IdleAction ed

    private void MoveAction()
    {
        if (player == null) return;

        float dx = player.position.x - transform.position.x;
        float distance = Mathf.Abs(dx);
        float dir = Mathf.Sign(dx);

        // 천천히 플레이어 방향으로 접근
        transform.Translate(Vector3.right * dir * walkSpeed * Time.deltaTime);
        animator.SetBool("Move", true);
    } // MoveAction ed

    private void AtkAction()
    {
        animator.SetBool("Move", false);
        if (isAtk || isCooling) return;
        isAtk = true;
        int pattern = Random.Range(0, 2);

        switch (pattern)
        {
            case 0:
                StartCoroutine(AtkRoutine("Atk1",atk1Damage,2f));
                break;
            case 1:
                StartCoroutine(AtkRoutine("Atk2",atk2Damage,3f));
                break;
        }
    } // AtkAction ed

    private IEnumerator AtkRoutine(string triggerName,int damage,float cooldown)
    {
        animator.SetTrigger(triggerName);
        yield return new WaitUntil(() => !isAtk);
        isCooling = true;
        yield return new WaitForSeconds(cooldown);
        isCooling = false;
        Debug.Log("쿨다운 종료");
        currentState = BossState.Move;
    }

    private void DieAction()
    {
        if (!Dead)
        {
            animator.Play("Gyuki_die");
            audioSource.PlayOneShot(dieClip);
            GameManager.Instance.AddGold(100);
            Destroy(gameObject, 3f);
            Dead = true;
        }
    }

    public void ActiveSkillCollider(int i) => atkColliders[i].SetActive(true);
    public void InactiveSkillCollider(int i) => atkColliders[i].SetActive(false);
    public void IsAtk() { isAtk = false; }
    public void AtkSound(int i) { audioSource.PlayOneShot(atkClips[i]); }
    
    public void TakeDamage(int damage) // 데미지 스크립트
    {
        BleedingEffect();
        HP -= damage; // 죽을시 TakeDamage 안되도록 **
        if (HP <= 0)
        {
            Debug.Log("규키. 스러지다.");
            FlagManager.Instance.SetFlag("GyukiTP", true);
        }
        else
        {
            Debug.Log("데미지! 규키 남은 체력 : " + HP);
        }
    }
    private void OnDrawGizmosSelected()
    {
        // 선택된 오브젝트(보스) 기준으로 기즈모 그리기

        // 플레이어 감지 범위: 파란색 원
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 공격 범위: 빨간색 원
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRange);
    }

    private void SpawnDustEffect()
    {
        if (dustPrefab != null && dustSpawnPoint != null)
        {
            GameObject dust = Instantiate(dustPrefab, dustSpawnPoint.position, dustSpawnPoint.rotation);
            Destroy(dust, 1f); // 1초 후 자동 제거
        }
    }
    private void BleedingEffect()
    {
        if (bleedingEffect != null)
        {
            GameObject vfx = Instantiate(bleedingEffect, transform.position, Quaternion.identity);
            Destroy(vfx, 1f);
        }
    } // BleedingEffect ed

} // class ed