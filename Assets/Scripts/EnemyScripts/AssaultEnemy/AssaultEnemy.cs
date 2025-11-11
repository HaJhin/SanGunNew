using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;

public class AssaultEnemy : MonoBehaviour, EnemyDamage
{
    public enum FSM {idle,Chase,Attack,Die}
    public FSM State = FSM.idle;
    private FSM prevState;

    public int HP = 20;
    public float chaseRange = 3f; // 추적 거리
    public float atkRange = 1.3f; // 공격 거리
    public float moveSpeed = 0.5f; // 이동속도
    public float attackCycle = 1f;
    public float currentCycle = 0f;

    public   bool canMove = true;
    public bool canAtk = true;

    public GameObject atkCollider;
    public Transform hitbox;
    public GameObject bleedingEffect;


    private Transform player;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private float attackDirX = 0f; // 공격 시작 시 방향을 저장

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip idleClip;
    public AudioClip atkClip;
    public AudioClip moveClip;
    public AudioClip hitClip;
    public AudioClip dieClip;

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
        Debug.Log(State);
        if (State == FSM.Die) return;
        CheckState();
        if (State != prevState) 
        {
            ExitState(prevState);
            EnterState(State);
            prevState = State;
        }
        UpdateState(State);
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
            State = FSM.Attack;
        }
        else if (distance <= chaseRange)
        {
            State = FSM.Chase;
        }
        else
        {
            if (State != FSM.idle)
            State = FSM.idle;
        } // if - else if - else ed

    } // CheckState ed

    private void EnterState(FSM newState)
    {
        switch(newState)
        {
            case FSM.idle:
                anim.SetBool("Chase", false);
                break;

            case FSM.Chase:
                audioSource.clip = moveClip;
                audioSource.Play();
                anim.SetBool("Chase", true);
                break;

            case FSM.Attack:
                break;

            case FSM.Die:
                anim.Play("Enemy1_die");
                audioSource.PlayOneShot(dieClip);
                GameManager.Instance.AddGold(Random.Range(3, 9)); // 재화 지급, 3~8 사이 랜덤.
                Destroy(gameObject, 0.9f);
                break;
        }
    }

    private void UpdateState(FSM currentState)
    {
        switch (currentState)
        {
            case FSM.Chase:
                ChasePlayer();
                break;
            case FSM.Attack:
                AtkPlayer();
                break;
        } // switch ed
    } // UpdateState ed 

    private void ExitState(FSM oldState)
    {
        switch (oldState)
        {
            case FSM.Chase:
                audioSource.Stop();
                audioSource.clip = null;
                anim.SetBool("Chase", false);
                break;
        } // switch ed
    } // ExitState ed

    // 플레이어를 향해 이동
    private void ChasePlayer()
    {
        if (canMove)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0f;

            transform.position += direction * moveSpeed * Time.deltaTime; // 직접 위치 이동
            FlipSprite(direction.x); // 방향 반전
        }
    } // ChasePlayer ed

    private void AtkPlayer()
    {
        if (canAtk)
        {
            attackDirX = player.position.x - transform.position.x;
            FlipSprite(attackDirX);
            canMove = false;
            canAtk = false;
            anim.SetTrigger("Attack");
        }
    }

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

   
    // 공격 제어 메서드
    public void CantAttack() { canAtk = true; canMove = true; attackDirX = 0f; }
    public void ActiveSkillCollider() => atkCollider.SetActive(true);
    public void AtkSound() => audioSource.PlayOneShot(atkClip);
    public void InactiveSkillCollider() => atkCollider.SetActive(false);

    public void TakeDamage(int damage) // 데미지 스크립트
    {
        BleedingEffect();
        canMove = false;
        HP -= damage; // 죽을시 TakeDamage 안되도록 **
        if (HP <= 0) {
            anim.Play("Enemy1_die"); // 사망 모션 재생
            Debug.Log("골드 지급!");
        }
        else { anim.Play("Enemy1_damage"); audioSource.PlayOneShot(hitClip); } // 아닐 시 데미지
        Debug.Log("데미지! 남은 체력 : " + HP);
    } // TakeDamage ed

    private void BleedingEffect()
    {
        if (bleedingEffect != null)
        {
            GameObject vfx = Instantiate(bleedingEffect,transform.position, Quaternion.identity);
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
