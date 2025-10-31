using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hidao : MonoBehaviour,EnemyDamage
{
    public enum BossState { Idle, Move, Atk, Die }
    public BossState currentState = BossState.Idle;

    public string flagName = "fight";
    public bool isFight = false;

    [Header("Stat")]
    public int HP = 30;

    [Header("Movement")]
    public float walkSpeed = 0.5f;
    public float detectionRange = 8f;
    public float atkRange = 2.5f;

    [Header("Atk Settings")]
    public int atk1Damage = 1;
    public int atk2Damage = 2;
    public int atk3Damage = 2;
    private bool isAtk = false;
    private bool isCooling = false;
    public GameObject[] atkColliders;

    private bool lockFacing = false;
    private bool lockMove = false;

    [Header("References")]
    private Transform player;
    private Animator animator;
    private SpriteRenderer sr;
    
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject go = GameObject.FindWithTag("Player");
            if (go != null) player = go.transform;
        }
    } // Start ed

    private void Update()
    {
        if (currentState != BossState.Die && isFight)
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
            Destroy(gameObject, 4f);
            return;
        }

        if (player == null) return;

        Vector3 diff = player.position - transform.position;
        diff.y = 0f; // 높이 무시한 평면 거리 계산
        float flatDistance = diff.magnitude;

        switch (currentState)
        {
            case BossState.Idle:
                if (flatDistance < detectionRange)
                    currentState = BossState.Move;
                break;

            case BossState.Move:
                if (flatDistance <= atkRange)
                    currentState = BossState.Atk;
                else if (flatDistance > detectionRange)
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
                break;
        }
    } // DoAction ed

    private void IdleAction()
    {
        animator.SetBool("Move", false);
        FacingPlayer();
    } // IdleAction ed

    private void MoveAction()
    {
        if (player == null || lockMove) return;
        animator.SetBool("Move", true); // 이동 애니 출력

        Vector3 diff = player.position - transform.position;
        diff.y = 0f;

        float zDiff = Mathf.Abs(diff.z);

        // 1) 먼저 Z축 정렬
        if (zDiff > 0.05f) // 정렬 허용 범위
        {
            Vector3 zDir = new Vector3(0, 0, diff.z).normalized;
            transform.Translate(zDir * walkSpeed * Time.deltaTime);

            // 정렬 중에는 바라보는 방향을 X축 비교로 유지
            if (!lockFacing &&  diff.x != 0)
                sr.flipX = diff.x > 0;

            return; // 아직 정렬 중이니 공격거리 판정은 하지 않음!
        }

        // 2) Z축이 정렬되었으니 X축으로 접근
        Vector3 xDir = new Vector3(diff.x, 0, 0).normalized;
        transform.Translate(xDir * walkSpeed * Time.deltaTime);

        if (!lockFacing) { FacingPlayer(); }
    } // MoveAction ed

    private void AtkAction()
    {
        animator.SetBool("Move", false);
        FacingPlayer();
        if (isAtk || isCooling) return;
        
        int pattern = Random.Range(0, 3);

        switch (pattern)
        {
            case 0:
                StartCoroutine(AtkRoutine("atk1", atk1Damage, 3f));
                break;
            case 1:
                StartCoroutine(AtkRoutine("atk2", atk2Damage, 5f));
                break;
            case 2:
                StartCoroutine(AtkRoutine("atk3", atk3Damage, 8f));
                break;
        }
    } // AtkAction ed

    private IEnumerator AtkRoutine(string triggerName, int damage, float cooldown)
    {
        lockMove = true;
        lockFacing = true;
        isAtk = true;

        animator.SetTrigger(triggerName);
        yield return new WaitUntil(() => !isAtk);

        lockMove = false;
        lockFacing = false;

        isCooling = true;
        yield return new WaitForSeconds(1.5f);
        isCooling = false;

        Debug.Log("쿨다운 종료");
        currentState = BossState.Idle;
    } // AtkRountine ed

    // 공격 제어 메서드 //
    public void ActiveSkillCollider(int i) => atkColliders[i].SetActive(true);
    public void InactiveSkillCollider(int i) => atkColliders[i].SetActive(false);
    public void IsAtk() { isAtk = false; }

    public void TakeDamage(int damage) // 데미지 스크립트
    {
        HP -= damage; // 죽을시 TakeDamage 안되도록 **
        if (HP <= 0)
        {
            animator.Play("Hidao_die"); // 사망 모션 재생
            Debug.Log("히다오. 스러지다.");
            FlagManager.Instance.SetFlag("HidaoTP", true);
        }
        else
        {
            Debug.Log("데미지! 히다오 남은 체력 : " + HP);
        }
    }

    private void FacingPlayer() // 플레이어 바라보기
    {
        if (lockFacing) return;

        Vector3 diff = player.position - transform.position;
        // 정면 방향 갱신
        if (diff.x != 0) sr.flipX = diff.x > 0;
        FlipHitbox();
    }
    private void FlipHitbox() // 히트박스 플립
    {
        foreach (GameObject col in atkColliders)
        {
            Vector3 pos = col.transform.localPosition;
            pos.x = Mathf.Abs(pos.x) * (sr.flipX ? 1 : -1);
            col.transform.localPosition = pos;
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

    private void OnEnable()
    {
        FlagManager.Instance.FlagChanged += OnFlagChanged;
    }
    private void OnDisable()
    {
        FlagManager.Instance.FlagChanged -= OnFlagChanged;
    }
    private void OnFlagChanged(string key, bool value)
    {
        if (key == flagName && value)
        {
            isFight = value;
        }
    } // OnFlagChanged ed
}
