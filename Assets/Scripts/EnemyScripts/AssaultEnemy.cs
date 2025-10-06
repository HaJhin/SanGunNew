using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultEnemy : MonoBehaviour, EnemyDamage
{
    public enum FSM {idle,Chase,Attack,Die}

    public FSM State = FSM.idle;

    public int HP = 20;
    public float chaseRange = 2.5f; // ���� �Ÿ�
    public float atkRange = 0.7f; // ���� �Ÿ�
    public float moveSpeed = 0.4f; // �̵��ӵ�
    public float attackCycle = 1f;
    public float currentCycle = 0f;

    public bool canAtk = true;
    public GameObject atkCollider;
    public Transform hitbox;

    private Transform player;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private float attackDirX = 0f; // ���� ���� �� ������ ����

    private void Awake()
    {
        anim = GetComponent<Animator>(); // �ִϸ����� �ʱ�ȭ
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��������Ʈ ������ �ʱ�ȭ
        player = GameObject.FindWithTag("Player").transform; // �÷��̾� �±׷� �÷��̾� ��ġ ���� �ʱ�ȭ
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
            Destroy(gameObject, 0.9f); // 1�� �� ����
            return;
        }

            float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= atkRange)
        {
            if (State != FSM.Attack) // ���� ���� ���� �� ���� 1ȸ�� ����
                attackDirX = player.position.x - transform.position.x;

            anim.SetBool("Attack", canAtk);
            anim.SetBool("Chase", false);
            State = FSM.Attack;
        }
        else if (distance <= chaseRange)
        {
            anim.SetBool("Attack",false);
            anim.SetBool("Chase", true);
            // ���� �ִϸ��̼� ��� �߿� ���� ���·� ���� ����
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
                // CheckState���� ó��
                break;
            default:
                break;
        } // switch ed
    } // Dostate ed

    // �÷��̾ ���� �̵�
    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        transform.position += direction * moveSpeed * Time.deltaTime; // ���� ��ġ �̵�
        FlipSprite(direction.x); // ���� ����
    } // ChasePlayer ed

    // ���� �� �÷��̾ �ٶ󺸰� �ϱ�
    private void FacePlayer()
    {
        FlipSprite(attackDirX); // �¿� ����
        // ���� ���� �����ϴ� �Լ� ���� �ִϸ��̺�Ʈ �������� ȣ���ؾ���
    } // FacePlayer ed

    // ��������Ʈ�� �¿� �����ϴ� �Լ�
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

    // ���� ��Ÿ���� Ȯ���ϴ� �޼���
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

    // ���� ���� �� ȣ�� - ��Ÿ�� ����
    public void CantAttack()
    {
        canAtk = false;
    }

    public void ActiveSkillCollider() => atkCollider.SetActive(true);
    public void InactiveSkillCollider() => atkCollider.SetActive(false);

    public void TakeDamage(int damage) // ������ ��ũ��Ʈ
    {
        HP -= damage; // ������ TakeDamage �ȵǵ��� **
        if (HP <= 0) {
            anim.Play("Enemy1_die"); // ��� ��� ���
            GameManager.Instance.AddGold(Random.Range(1, 6)); // ��ȭ ����, 1~5 ���� ����.
            Debug.Log("��� ����!");
        }
        else { anim.Play("Enemy1_damage"); } // �ƴ� �� ������
        Debug.Log("������! ���� ü�� : " + HP);
    }
}
