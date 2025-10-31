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
        diff.y = 0f; // ���� ������ ��� �Ÿ� ���
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
        animator.SetBool("Move", true); // �̵� �ִ� ���

        Vector3 diff = player.position - transform.position;
        diff.y = 0f;

        float zDiff = Mathf.Abs(diff.z);

        // 1) ���� Z�� ����
        if (zDiff > 0.05f) // ���� ��� ����
        {
            Vector3 zDir = new Vector3(0, 0, diff.z).normalized;
            transform.Translate(zDir * walkSpeed * Time.deltaTime);

            // ���� �߿��� �ٶ󺸴� ������ X�� �񱳷� ����
            if (!lockFacing &&  diff.x != 0)
                sr.flipX = diff.x > 0;

            return; // ���� ���� ���̴� ���ݰŸ� ������ ���� ����!
        }

        // 2) Z���� ���ĵǾ����� X������ ����
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

        Debug.Log("��ٿ� ����");
        currentState = BossState.Idle;
    } // AtkRountine ed

    // ���� ���� �޼��� //
    public void ActiveSkillCollider(int i) => atkColliders[i].SetActive(true);
    public void InactiveSkillCollider(int i) => atkColliders[i].SetActive(false);
    public void IsAtk() { isAtk = false; }

    public void TakeDamage(int damage) // ������ ��ũ��Ʈ
    {
        HP -= damage; // ������ TakeDamage �ȵǵ��� **
        if (HP <= 0)
        {
            animator.Play("Hidao_die"); // ��� ��� ���
            Debug.Log("���ٿ�. ��������.");
            FlagManager.Instance.SetFlag("HidaoTP", true);
        }
        else
        {
            Debug.Log("������! ���ٿ� ���� ü�� : " + HP);
        }
    }

    private void FacingPlayer() // �÷��̾� �ٶ󺸱�
    {
        if (lockFacing) return;

        Vector3 diff = player.position - transform.position;
        // ���� ���� ����
        if (diff.x != 0) sr.flipX = diff.x > 0;
        FlipHitbox();
    }
    private void FlipHitbox() // ��Ʈ�ڽ� �ø�
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
        // ���õ� ������Ʈ(����) �������� ����� �׸���

        // �÷��̾� ���� ����: �Ķ��� ��
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // ���� ����: ������ ��
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
