using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeEnemy : MonoBehaviour, EnemyDamage
{
    // 상태 관련
    public enum State { Idle, Chase, Attack, Die }
    State currentState = State.Idle;
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

    [Header("References")]
    private Transform player; // 플레이어의 위치
    

    private void Awake()
    {
        anim = GetComponent<Animator>(); // 애니메이터 초기화
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 초기화
        player = GameObject.FindWithTag("Player").transform; // 플레이어 태그로 플레이어 위치 정보 초기화
    } // Awake ed

    private void Update()
    {
        if (currentState != State.Die)
        {
            CheckState();
            DoState();
        }
    }

    private void CheckState() // State 확인 및 갱신
    {
        float dist = Vector3.Distance(transform.position, player.position); // 플레이어와의 거리 계산

        // State 전환
        if (HP <= 0) currentState = State.Die;
        else if (dist <= atkRange) currentState = State.Attack;
        else if (dist <= chaseRange) currentState = State.Chase;
        else currentState = State.Idle;
    } // CheckState ed

    private void DoState() // State 실행
    {
        // State 수행
        switch (currentState)
        {
            case State.Idle:
                anim.SetBool("move", false);
                anim.SetBool("atk", false);
                Debug.Log("현 상태 Idle");
                break;
            case State.Chase:
                anim.SetBool("move", true);
                anim.SetBool("atk", false);
                RangeEnemyMove();
                Debug.Log("현 상태 Chase");
                break;
            case State.Attack:
                anim.SetBool("move", false);
                anim.SetBool("atk", true );
                Debug.Log("현 상태 Attack");
                break;
            case State.Die:
                anim.Play("Enemy2_Die");
                Destroy(gameObject, 2f); // 1초 후 제거
                Debug.Log("현 상태 Die");
                break;
        } // Switch ed
    } // DoState ed

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
        HP -= damage; // 죽을시 TakeDamage 안되도록 **
        if (HP <= 0)
        {
            anim.Play("Enemy2_die"); // 사망 모션 재생
            GameManager.Instance.AddGold(Random.Range(1, 6)); // 재화 지급, 1~5 사이 랜덤.
            Debug.Log("골드 지급!");
        }
        else 
        { 
            anim.Play("Enemy2_damage");
            currentState = State.Idle;
        } // 아닐 시 데미지
        
        Debug.Log("데미지! 남은 체력 : " + HP);
    }
}
