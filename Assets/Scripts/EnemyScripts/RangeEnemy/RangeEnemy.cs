using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;

public class RangeEnemy : MonoBehaviour, EnemyDamage
{
    // 상태 관련
    public enum State { Idle, Chase, Attack, Die }
    State currentState = State.Idle;
    private State prevState;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    

    [Header("Stats")]
    public int HP = 15; // 체력
    public float moveSpeed = 0.2f; // 이동속도
    public int atkDamage = 1; // 공격력

    [Header("Range Settings")]
    public float chaseRange = 6f; // 추적 거리
    public float atkRange = 4f; // 공격 거리

    [Header("Attack Settings")]
    public bool canAtk = true; // 공격 가능 여부 (애니메이션 제어)
    public GameObject projectile; // 투사체 프리펩
    public Transform firePoint; // 투사체 발사 위치

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip moveClip;
    public AudioClip AtkClip;
    public AudioClip hitClip;
    public AudioClip dieClip;

    [Header("References")]
    private Transform player; // 플레이어의 위치
    public GameObject bleedingEffect;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>(); // 애니메이터 초기화
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 초기화
        player = GameObject.FindWithTag("Player").transform; // 플레이어 태그로 플레이어 위치 정보 초기화
    } // Awake ed

    private void Update()
    {
        CheckPause();
        if (currentState == State.Die) return;

        if (currentState != State.Die)
        {
            CheckState();
            if (currentState != prevState)
            {
                ExitState(prevState);
                EnterState(currentState);
                prevState = currentState;
            }
            UpdateState(currentState);
        }
    }

    private void CheckState() // State 확인 및 갱신
    {
        float dist = Vector3.Distance(transform.position, player.position); // 플레이어와의 거리 계산

        // State 전환
        if (HP <= 0)
        {
            currentState = State.Die;
            return;
        }

        if (currentState != State.Die)
        {
            if (dist <= atkRange) currentState = State.Attack;
            else if (dist <= chaseRange) currentState = State.Chase;
            else currentState = State.Idle;
        }
    } // CheckState ed

    private void EnterState(State newState) // State 진입
    {
        // State 수행
        switch (newState)
        {
            case State.Idle:
                anim.SetBool("move", false);
                anim.SetBool("atk", false);
                break;
            case State.Chase:
                audioSource.clip = moveClip;
                audioSource.Play();
                anim.SetBool("move", true);
                anim.SetBool("atk", false);
                break;
            case State.Attack:
                anim.SetBool("move", false);
                anim.SetBool("atk", true );
                break;
            case State.Die:
                audioSource.PlayOneShot(dieClip);
                GameManager.Instance.AddGold(Random.Range(2,8)); // 재화 지급, 2~7 사이 랜덤.
                Destroy(gameObject, 2f); // 1초 후 제거
                break;
        } // Switch ed
    } // EnterState ed

    public void UpdateState(State currentState) 
    {
        switch (currentState)
        {
            case State.Chase:
                RangeEnemyMove();
                break;
        } // switch ed
    }  // updateState ed

    public void ExitState(State oldState)
    {
        switch (oldState)
        {
            case State.Chase:
                audioSource.Stop();
                audioSource.clip = null;
                break;
        } // switch ed
    } // ExitState ed

    void RangeEnemyAtk() // 공격 메서드
    {
        if (canAtk)
        {
            GameObject proj = Instantiate(projectile, firePoint.position, Quaternion.identity); // 투사체 Instantiate

            Vector3 targetPos = player.position; // 플레이어의 현 위치 = targetPos 저장
            Vector3 dir = (targetPos - firePoint.position).normalized; // 플레이어의 위치와 발사지점을 토대로 방향 설정

            RangeProjectile po = proj.GetComponent<RangeProjectile>();
            if (po != null)
            {
                po.Initialize(dir, atkDamage);
            }
            canAtk = false;
            Debug.Log("발사!!");
        }
    }
    void RecoveryAtk(){if (!canAtk) canAtk = true;} // 공격 회복

    void AtkSound() { audioSource.PlayOneShot(AtkClip); }

    void RangeEnemyMove() // 이동 메서드
    {
        // 직접 위치 이동
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        if (direction.x > 0) spriteRenderer.flipX = true;
        else if (direction.x < 0) spriteRenderer.flipX = false;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    void ImageFlip() // 스프라이트 플립
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        if (direction.x > 0) spriteRenderer.flipX = true;
        else if (direction.x < 0) spriteRenderer.flipX = false;
    }

    public void TakeDamage(int damage) // 데미지 스크립트
    {
        BleedingEffect();
        HP -= damage; // 죽을시 TakeDamage 안되도록 **
        if (HP <= 0)
        {
            anim.Play("Enemy2_die");
            Debug.Log("캇파 처치");
        }
        else 
        { 
            anim.Play("Enemy2_damage");
            audioSource.PlayOneShot(hitClip);
            currentState = State.Idle;
        } // 아닐 시 데미지
        
        
    }
    private void BleedingEffect()
    {
        if (bleedingEffect != null)
        {
            GameObject vfx = Instantiate(bleedingEffect, transform.position, Quaternion.identity);
            Destroy(vfx, 1f);
        }
    } // BleedingEffect ed
    private void CheckPause()
    {
        if (GameManager.Instance.pauseNow)
        {
            audioSource.Stop();
        }
    }
}
