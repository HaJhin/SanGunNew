using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Movement Settings")]
    public float moveSpeed = 1.5f; // 이동속도
    private Vector3 moveInput; // 이동방향

    [Header("Dash Settings")]
    public float dashSpeed = 4f; // 대쉬속도
    public float dashDuration = 0.25f; // 대쉬지속시간
    private Vector3 dashDirection; // 대쉬방향
    private float dashTimer; // 대쉬쿨타임

    public bool DashAtk = false; // 대시공격 가능 여부

    [Header("Attack Settings")]
    public GameObject[] skillColliders; // 공격 히트박스
    public int maxComboLevel = 1; // 현재 콤보 레벨(1~3)
    public int currentCombo = 0; // 현재 콤보 카운트
    public float comboResetTime = 0.8f; // 콤보 초기화 시간
    private float comboTimer; // 콤보 타이머

    [Header("Visual Components")]
    public SpriteRenderer spriteRenderer; // 스프라이트렌더러
    public Animator animator; // 애니메이터
    public Transform hitBox; // 히트박스 컴포넨트

    [Header("System Components")]
    private Rigidbody rb; // 리지드바디
    private Collider playerCollider; // 플레이어 본체 콜라이더

    // 상태 정의
    private enum PlayerState {idle,Move,Atk,Dash,Dead}
    private PlayerState currentState = PlayerState.idle;

    private void Awake() // 초기화
    {
        Instance = this;

        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponentInChildren<Collider>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    } // Awake ed

    private void Update() // 메인 루프
    {
        if (currentState == PlayerState.Dead) return;

        if (GameManager.Instance.GameOver && currentState != PlayerState.Dead)
        {
            ChangeState(PlayerState.Dead);
            playerCollider.enabled = false;
            Destroy(this);
            return;
        } // 게임오버 처리

        HandleInput();
        HandleState();

        // 콤보 타이머 갱신
        if (currentCombo > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > comboResetTime)
            {
                currentCombo = 0; // 일정 시간 지나면 콤보 초기화
            }
        }
    } // Update ed

    void HandleInput() // 플레이어 조작 제어
    {
        if (currentState == PlayerState.Dead) return; // 게임 오버 시 조작 불가

        // 이동 방향 조작(십자키)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(h, 0, v).normalized;

        // 공격 조작
        if (Input.GetMouseButtonDown(0) && currentState != PlayerState.Atk && currentState != PlayerState.Dash)
        { StartComboAtk(); }

        // 대시 조작
        if (Input.GetKeyDown(KeyCode.LeftShift) && moveInput.magnitude > 0.1f && currentState == PlayerState.Move)
        {ChangeState(PlayerState.Dash);}

        // 회복 소모템 사용
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {Inventory.Instance.UseItem(ItemType.Heal);}

        // 쉴드 소모템 사용
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {Inventory.Instance.UseItem(ItemType.Shield);}
    } // HandleInput ed

    void HandleState() // 상태별 메서드 처리
    {
        switch (currentState)
        {
            case PlayerState.idle:
                if (moveInput.magnitude > 0.1f) ChangeState(PlayerState.Move); // 조작키 입력시 이동 상태 전환
                break;
            case PlayerState.Move:
                HandleMovement(); // 이동 제어
                break;
            case PlayerState.Atk:
                // 애니메이션 이벤트로 처리
                break;
            case PlayerState.Dash:
                HandleDash(); // 대쉬 제어
                break;
        } // switch ed
    } // HandleState ed

    void ChangeState(PlayerState newState) // 상태별 애니 전환 메서드
    {
        if (currentState == newState) return;
        currentState = newState;

        switch (newState)
        {
            case PlayerState.idle:
                animator.SetBool("isMoving",false); 
                break;
            case PlayerState.Move:
                animator.SetBool("isMoving", true);
                break;
            case PlayerState.Atk:
                animator.SetBool("isMoving", false);
                animator.Play($"Player_Atk{currentCombo}"); // 단계별 애니 삽입
                break;
            case PlayerState.Dead:
                animator.Play("Player_die");
                break;
        } // switch ed
    } // ChangeState ed

    void HandleMovement() // 이동 메서드
    {
        if (moveInput.magnitude <= 0.1f)
        {
            ChangeState(PlayerState.idle);
            return;
        }
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.deltaTime); // 이동(rb기반)
        // 방향 전환
        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x > 0; // 스프라이트 전환
            // 히트박스 방향 전환
            Vector3 pos = hitBox.localPosition;
            pos.x = Mathf.Abs(pos.x) * (moveInput.x < 0 ? -1 : 1);
            hitBox.localPosition = pos;
        } // if ed
    } // HandleMovement ed

    void HandleDash() // 대쉬 메서드
    {
        if (dashTimer == 0f)
        {
            dashDirection = moveInput.normalized;
            animator.SetTrigger("Dash");

            // 대쉬 시작 시 콜라이더 비활성화
            Collider col = GetComponent<Collider>();
            if (col != null)
                col.enabled = false;
        }

        dashTimer += Time.deltaTime;
        rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.deltaTime);

        if (dashTimer >= dashDuration)
        {
            dashTimer = 0f;

            // 대쉬 종료 시 콜라이더 활성화
            Collider col = GetComponent<Collider>();
            if (col != null)
                col.enabled = true;

            ChangeState(PlayerState.idle);
        }
    }

    // 공격 처리 메서드(콤보)
    void StartComboAtk()
    {
        if (currentCombo < maxComboLevel) // 현재 콤보카운트가 콤보레벨보다 낮을 시
        {
            currentCombo++; // 콤보카운트 +1
            comboTimer = 0f; // 콤보타이머 초기화
            ChangeState(PlayerState.Atk); // 상태 전환
        } else
        {
            currentCombo = 1; // 콤보 리셋
            comboTimer = 0f; // 콤보타이머 초기화
            ChangeState(PlayerState.Atk);
        }
    } // StartComboAtk ed

    // 공격 콜라이더 제어
    public void ActiveSkillCollider(int idx) => skillColliders[idx].SetActive(true);
    public void InactiveSkillCollider(int idx) => skillColliders[idx].SetActive(false);
    public void CanMove() {if (currentState == PlayerState.Atk) ChangeState(PlayerState.idle);}

    // 커서 숨기기
    private void OnApplicationFocus(bool focus){Cursor.lockState = focus ? CursorLockMode.Locked : CursorLockMode.None;}

} // class ed
