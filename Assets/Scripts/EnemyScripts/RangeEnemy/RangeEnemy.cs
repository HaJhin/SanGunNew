using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeEnemy : MonoBehaviour, EnemyDamage
{
    // ���� ����
    public enum State { Idle, Chase, Attack, Die }
    State currentState = State.Idle;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    [Header("Stats")]
    public int HP = 15; // ü��
    public float moveSpeed = 0.2f; // �̵��ӵ�
    public int atkDamage = 1; // ���ݷ�

    [Header("Range Settings")]
    public float chaseRange = 6f; // ���� �Ÿ�
    public float atkRange = 4f; // ���� �Ÿ�

    [Header("Attack Settings")]
    public bool canAtk = true; // ���� ���� ���� (�ִϸ��̼� ����)
    public GameObject projectile; // ����ü ������
    public Transform firePoint; // ����ü �߻� ��ġ

    [Header("References")]
    private Transform player; // �÷��̾��� ��ġ
    

    private void Awake()
    {
        anim = GetComponent<Animator>(); // �ִϸ����� �ʱ�ȭ
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��������Ʈ ������ �ʱ�ȭ
        player = GameObject.FindWithTag("Player").transform; // �÷��̾� �±׷� �÷��̾� ��ġ ���� �ʱ�ȭ
    } // Awake ed

    private void Update()
    {
        if (currentState != State.Die)
        {
            CheckState();
            DoState();
        }
    }

    private void CheckState() // State Ȯ�� �� ����
    {
        float dist = Vector3.Distance(transform.position, player.position); // �÷��̾���� �Ÿ� ���

        // State ��ȯ
        if (HP <= 0) currentState = State.Die;
        else if (dist <= atkRange) currentState = State.Attack;
        else if (dist <= chaseRange) currentState = State.Chase;
        else currentState = State.Idle;
    } // CheckState ed

    private void DoState() // State ����
    {
        // State ����
        switch (currentState)
        {
            case State.Idle:
                anim.SetBool("move", false);
                anim.SetBool("atk", false);
                Debug.Log("�� ���� Idle");
                break;
            case State.Chase:
                anim.SetBool("move", true);
                anim.SetBool("atk", false);
                RangeEnemyMove();
                Debug.Log("�� ���� Chase");
                break;
            case State.Attack:
                anim.SetBool("move", false);
                anim.SetBool("atk", true );
                Debug.Log("�� ���� Attack");
                break;
            case State.Die:
                anim.Play("Enemy2_Die");
                Destroy(gameObject, 2f); // 1�� �� ����
                Debug.Log("�� ���� Die");
                break;
        } // Switch ed
    } // DoState ed

    void RangeEnemyAtk() // ���� �޼���
    {
        if (canAtk)
        {
            GameObject proj = Instantiate(projectile, firePoint.position, Quaternion.identity); // ����ü Instantiate

            Vector3 targetPos = player.position; // �÷��̾��� �� ��ġ = targetPos ����
            Vector3 dir = (targetPos - firePoint.position).normalized; // �÷��̾��� ��ġ�� �߻������� ���� ���� ����

            RangeProjectile po = proj.GetComponent<RangeProjectile>();
            if (po != null)
            {
                po.Initialize(dir, atkDamage);
            }
            canAtk = false;
            Debug.Log("�߻�!!");
        }
    }
    void RecoveryAtk(){if (!canAtk) canAtk = true;} // ���� ȸ��

    void RangeEnemyMove() // �̵� �޼���
    {
        // ���� ��ġ �̵�
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        if (direction.x > 0) spriteRenderer.flipX = true;
        else if (direction.x < 0) spriteRenderer.flipX = false;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    void ImageFlip() // ��������Ʈ �ø�
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        if (direction.x > 0) spriteRenderer.flipX = true;
        else if (direction.x < 0) spriteRenderer.flipX = false;
    }

    public void TakeDamage(int damage) // ������ ��ũ��Ʈ
    {
        HP -= damage; // ������ TakeDamage �ȵǵ��� **
        if (HP <= 0)
        {
            anim.Play("Enemy2_die"); // ��� ��� ���
            GameManager.Instance.AddGold(Random.Range(1, 6)); // ��ȭ ����, 1~5 ���� ����.
            Debug.Log("��� ����!");
        }
        else 
        { 
            anim.Play("Enemy2_damage");
            currentState = State.Idle;
        } // �ƴ� �� ������
        
        Debug.Log("������! ���� ü�� : " + HP);
    }
}
