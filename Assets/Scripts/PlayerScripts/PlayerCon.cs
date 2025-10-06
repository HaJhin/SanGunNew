using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    public float moveSpeed = 1.5f; // 이동 속도
    public GameObject[] skillColliders; // 스킬 타격 판정
    public SpriteRenderer spriteRenderer; // 스프라이트 방향 반전을 위한 렌더러
    public Animator animator; // 애니메이션 컨트롤러
    public Transform hitbox; // 히트박스 오브젝트의 Transform (빈 오브젝트)

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

            // 스프라이트 반전 (좌우 방향만 판단)
            if (h != 0)
            {
                spriteRenderer.flipX = h > 0; // 이미지 반전
                                              // 히트박스 위치 반전
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

            animator.SetBool("isDash", true); // ← 애니메이션 트리거
            animator.SetBool("isMoving", false); // ← 중복 방지
        }

        // 대시 중
        if (isDashing)
        {
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            dashTimer += Time.deltaTime;

            if (dashTimer >= dashDuration)
            {
                isDashing = false;
                canMove = true;

                animator.SetBool("isDash", false); // ← 복귀
            }
        }

        // 기본 공격
        if (Input.GetMouseButtonDown(0) && !isAtk)
        {
            isAtk = true;
            canMove = false;
            animator.Play("Player_Atk1");
        }

        // 대시
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            isDashing = true;
            canMove = false;
            animator.Play("Player_Dash");
            Vector3 dashDir = moveInput.normalized;
            float dashDistance = dashSpeed * dashDuration;
            transform.position += dashDir * dashDistance;
        }

        
         // 소모품 체력 회복   
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("회복 아이템 쓰고 싶어...");
            Inventory.Instance.UseItem(ItemType.Heal);
        }

        // 소모품 쉴드 생성
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Inventory.Instance.UseItem(ItemType.Shield);
        }
         
} // update ed

    public void ActiveSkillCollider(int idx) => skillColliders[idx].SetActive(true); // 공격 힛박 활성화

    public void InactiveSkillCollider(int idx) => skillColliders[idx].SetActive(false); // 공격 힛박 비활성화

    public void Canmove() // 이동 가능
    {
        isAtk = false;
        canMove = true;
    } // Canmove ed

    private void OnApplicationFocus(bool focus) // 커서 숨기기 스크립트
    {
        // 어플리케이션이 포커스를 받으면 커서를 숨긴다.
        if (focus) { Cursor.lockState = CursorLockMode.Locked; }
        else { Cursor.lockState = CursorLockMode.None; }
    }

} // PlayerCon ed
