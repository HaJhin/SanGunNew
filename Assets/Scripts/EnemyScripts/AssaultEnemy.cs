using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultEnemy : MonoBehaviour, EnemyDamage
{
    public enum FSM {idle,Chase,Attack,Die}

    public FSM State = FSM.idle;

    public int HP = 20;
    public float chaseRange = 2.5f; // 추적 거리
    public float atkRange = 0.7f; // 공격 거리
    public float moveSpeed = 0.4f; // 이동속도
    public float attackCycle = 1f;
    public float currentCycle = 0f;

    public bool canAtk = true;
    public GameObject atkCollider;
    public Transform hitbox;

    private Transform player;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private float attackDirX = 0f; // 공격 시작 시 방향을 저장

    private void Awake()
    {
        anim = GetComponent<Animator>(); // 애니메이터 초기화
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 초기화
        player = GameObject.FindWithTag("Player").transform; // 플레이어 태그로 플레이어 위치 정보 초기화
    } // Awake ed

    private void Update()
    {
        if (State != FSM.Die)
        {
            CheckState();
            CheckAttackCycle();
            DoState();
        }
    } // Update ed

    private void CheckState()
    {
        if (HP <= 0)
        {
            State = FSM.Die;
            Destroy(gameObject, 0.9f); // 1초 후 제거
            return;
        }

            float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= atkRange)
        {
            if (State != FSM.Attack) // 공격 상태 진입 시 최초 1회만 저장
                attackDirX = player.position.x - transform.position.x;

            anim.SetBool("Attack", canAtk);
            anim.SetBool("Chase", false);
            State = FSM.Attack;
        }
        else if (distance <= chaseRange)
        {
            anim.SetBool("Attack",false);
            anim.SetBool("Chase", true);
            // 공격 애니메이션 재생 중엔 추적 상태로 가지 않음
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy1_Atk"))
            State = FSM.Chase;
        }
        else
        {
            anim.SetBool("Attack", false);
            anim.SetBool("Chase", false);
            State = FSM.idle;
        } // if - else if - else ed

    } // CheckState ed

    private void DoState()
    {
        switch (State)
        {
            case FSM.idle:
                break;
            case FSM.Chase:
                ChasePlayer();
                break;
            case FSM.Attack:
                FacePlayer();
                break;
            case FSM.Die:
                // CheckState에서 처리
                break;
            default:
                break;
        } // switch ed
    } // Dostate ed

    // 플레이어를 향해 이동
    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        transform.position += direction * moveSpeed * Time.deltaTime; // 직접 위치 이동
        FlipSprite(direction.x); // 방향 반전
    } // ChasePlayer ed

    // 공격 시 플레이어를 바라보게 하기
    private void FacePlayer()
    {
        FlipSprite(attackDirX); // 좌우 반전
        // 공격 방향 리셋하는 함수 만들어서 애니메이벤트 마지막에 호출해야함
    } // FacePlayer ed

    // 스프라이트를 좌우 반전하는 함수
    private void FlipSprite(float xDir)
    {
        if (xDir > 0.01f)
        { spriteRenderer.flipX = true; }
        else if (xDir < -0.01f)
        { spriteRenderer.flipX = false; }
        Vector3 pos = hitbox.localPosition;
        pos.x = Mathf.Abs(pos.x) * (xDir < 0 ? -1 : 1);
        hitbox.localPosition = pos;
    } // FlipSprite ed

    // 공격 쿨타임을 확인하는 메서드
    private void CheckAttackCycle()
    {
        if (!canAtk)
        {
            currentCycle += Time.deltaTime;

            if (currentCycle >= attackCycle)
            {
                canAtk = true;
                currentCycle = 0f;
            } 
        } // if ed
    } // CAC ed

    // 공격 종료 후 호출 - 쿨타임 시작
    public void CantAttack()
    {
        canAtk = false;
    }

    public void ActiveSkillCollider() => atkCollider.SetActive(true);
    public void InactiveSkillCollider() => atkCollider.SetActive(false);

    public void TakeDamage(int damage) // 데미지 스크립트
    {
        HP -= damage; // 죽을시 TakeDamage 안되도록 **
        if (HP <= 0) {
            anim.Play("Enemy1_die"); // 사망 모션 재생
            GameManager.Instance.AddGold(Random.Range(1, 6)); // 재화 지급, 1~5 사이 랜덤.
            Debug.Log("골드 지급!");
        }
        else { anim.Play("Enemy1_damage"); } // 아닐 시 데미지
        Debug.Log("데미지! 남은 체력 : " + HP);
    }
}
