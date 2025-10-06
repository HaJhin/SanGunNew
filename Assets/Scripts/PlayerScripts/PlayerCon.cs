using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    public float moveSpeed = 1.5f; // �̵� �ӵ�
    public GameObject[] skillColliders; // ��ų Ÿ�� ����
    public SpriteRenderer spriteRenderer; // ��������Ʈ ���� ������ ���� ������
    public Animator animator; // �ִϸ��̼� ��Ʈ�ѷ�
    public Transform hitbox; // ��Ʈ�ڽ� ������Ʈ�� Transform (�� ������Ʈ)

    private Rigidbody rb;
    private Vector3 moveInput;  
    private bool isAtk = false;
    private bool canMove = true;

    private bool isDashing = false;
    private float dashTimer = 0f;
    public float dashDuration = 0.25f;
    public float dashSpeed = 4f;
    private Vector3 dashDirection = Vector3.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameOver)
        {
            animator.Play("Player_die");
            Destroy(this);
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(h, 0, v).normalized * moveSpeed;
        if (canMove)
        {
            animator.SetBool("isMoving", moveInput.magnitude > 0.01f);
            transform.position += moveInput * Time.deltaTime;

            // ��������Ʈ ���� (�¿� ���⸸ �Ǵ�)
            if (h != 0)
            {
                spriteRenderer.flipX = h > 0; // �̹��� ����
                                              // ��Ʈ�ڽ� ��ġ ����
                Vector3 pos = hitbox.localPosition;
                pos.x = Mathf.Abs(pos.x) * (h < 0 ? -1 : 1);
                hitbox.localPosition = pos;
            } // if ed
        }

        if (!isDashing && Input.GetKeyDown(KeyCode.LeftShift) && moveInput.magnitude > 0.1f && canMove && !isAtk)
        {
            isDashing = true;
            canMove = false;
            dashTimer = 0f;
            dashDirection = moveInput.normalized;

            animator.SetBool("isDash", true); // �� �ִϸ��̼� Ʈ����
            animator.SetBool("isMoving", false); // �� �ߺ� ����
        }

        // ��� ��
        if (isDashing)
        {
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            dashTimer += Time.deltaTime;

            if (dashTimer >= dashDuration)
            {
                isDashing = false;
                canMove = true;

                animator.SetBool("isDash", false); // �� ����
            }
        }

        // �⺻ ����
        if (Input.GetMouseButtonDown(0) && !isAtk)
        {
            isAtk = true;
            canMove = false;
            animator.Play("Player_Atk1");
        }

        // ���
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            isDashing = true;
            canMove = false;
            animator.Play("Player_Dash");
            Vector3 dashDir = moveInput.normalized;
            float dashDistance = dashSpeed * dashDuration;
            transform.position += dashDir * dashDistance;
        }

        
         // �Ҹ�ǰ ü�� ȸ��   
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("ȸ�� ������ ���� �;�...");
            Inventory.Instance.UseItem(ItemType.Heal);
        }

        // �Ҹ�ǰ ���� ����
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Inventory.Instance.UseItem(ItemType.Shield);
        }
         
} // update ed

    public void ActiveSkillCollider(int idx) => skillColliders[idx].SetActive(true); // ���� ���� Ȱ��ȭ

    public void InactiveSkillCollider(int idx) => skillColliders[idx].SetActive(false); // ���� ���� ��Ȱ��ȭ

    public void Canmove() // �̵� ����
    {
        isAtk = false;
        canMove = true;
    } // Canmove ed

    private void OnApplicationFocus(bool focus) // Ŀ�� ����� ��ũ��Ʈ
    {
        // ���ø����̼��� ��Ŀ���� ������ Ŀ���� �����.
        if (focus) { Cursor.lockState = CursorLockMode.Locked; }
        else { Cursor.lockState = CursorLockMode.None; }
    }

} // PlayerCon ed
