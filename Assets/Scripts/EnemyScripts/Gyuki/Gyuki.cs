using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gyuki : MonoBehaviour , EnemyDamage
{
    public enum BossState { Idle, Move, Atk, Die }
    public BossState currentState = BossState.Idle;

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

    [Header("Sprite Color")]
    private SpriteRenderer sr;
    private Color originalColor;
    private Color flashColor;
    private string flashHexColor = "FFB8B8";

    [Header("UI Components")]
    public Slider healthSlider; // 규키 체력바 슬라이더

    [Header("Effect")]
    public GameObject dustPrefab;
    public Transform dustSpawnPoint;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    private void Start()
    {
        healthSlider.maxValue = HP;
        healthSlider.value = HP;

        if (player == null)
        {
            GameObject go = GameObject.FindWithTag("Player");
            if (go != null) player = go.transform;
        }

        if (!ColorUtility.TryParseHtmlString(flashHexColor, out flashColor))
        {
            flashColor = Color.red;
            Debug.LogWarning("헥스 색상 파싱 실패. 기본색(빨강)으로 설정.");
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
           Destroy(gameObject, 2f);
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
        int pattern = Random.Range(0, 3);

        switch (pattern)
        {
            case 0:
                StartCoroutine(AtkRoutine("Atk1",atk1Damage,2f));
                break;
            case 1:
                StartCoroutine(AtkRoutine("Atk2",atk2Damage,3f));
                break;
            case 2:
                StartCoroutine(Backstep());
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

    private IEnumerator Backstep()
    {
        isAtk = true;
        animator.Play("Gyuki_backstep");
        Vector3 start = transform.position;
        Vector3 end = start - -transform.right * backstepDistance;
        float time = 2.0f;
        for (float t = 0; t < 1f; t += Time.deltaTime / time)
        {
            transform.position = Vector3.Lerp(start, end, Mathf.SmoothStep(t, 1, 0));
            yield return null;
        }
        transform.position = end;
        yield return new WaitUntil(() => !isAtk);       
        yield return new WaitForSeconds(1f); // 여운 시간
        isAtk = false;
        currentState = BossState.Move;  
    } // Backstep ed

    public void ActiveSkillCollider(int i) => atkColliders[i].SetActive(true);
    public void InactiveSkillCollider(int i) => atkColliders[i].SetActive(false);

    public void IsAtk() { isAtk = false; }
    
    public void TakeDamage(int damage) // 데미지 스크립트
    {
        HP -= damage; // 죽을시 TakeDamage 안되도록 **
        if (HP <= 0)
        {
            Destroy(healthSlider);
            animator.Play("Gyuki_die"); // 사망 모션 재생
            GameManager.Instance.AddGold(100); // 재화 지급, 1~5 사이 랜덤.
            Debug.Log("규키. 스러지다.");
            FlagManager.Instance.SetFlag("GyukiTP", true);
        }
        else
        {
            Debug.Log("데미지! 규키 남은 체력 : " + HP);
            healthSlider.value = HP;
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
            Instantiate(dustPrefab, dustSpawnPoint.position, dustSpawnPoint.rotation);
            Destroy(dustPrefab, 1f); // 1초 후 자동 제거
        }
    }

    private IEnumerator FlashWhite()
    {
        sr.color = flashColor;
        Debug.Log("깜빡!");
        yield return new WaitForSeconds(0.2f);
        sr.color = originalColor;
    }
} // class ed