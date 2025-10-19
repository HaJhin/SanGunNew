using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Movement Settings")]
    public float moveSpeed = 1.5f; // �̵��ӵ�
    private Vector3 moveInput; // �̵�����

    [Header("Dash Settings")]
    public float dashSpeed = 4f; // �뽬�ӵ�
    public float dashDuration = 0.25f; // �뽬���ӽð�
    private Vector3 dashDirection; // �뽬����
    private float dashTimer; // �뽬��Ÿ��

    public bool DashAtk = false; // ��ð��� ���� ����

    [Header("Attack Settings")]
    public GameObject[] skillColliders; // ���� ��Ʈ�ڽ�
    public int maxComboLevel = 1; // ���� �޺� ����(1~3)
    public int currentCombo = 0; // ���� �޺� ī��Ʈ
    public float comboResetTime = 0.8f; // �޺� �ʱ�ȭ �ð�
    private float comboTimer; // �޺� Ÿ�̸�

    [Header("Visual Components")]
    public SpriteRenderer spriteRenderer; // ��������Ʈ������
    public Animator animator; // �ִϸ�����
    public Transform hitBox; // ��Ʈ�ڽ� ������Ʈ

    [Header("System Components")]
    private Rigidbody rb; // ������ٵ�
    private Collider playerCollider; // �÷��̾� ��ü �ݶ��̴�

    // ���� ����
    private enum PlayerState {idle,Move,Atk,Dash,Dead}
    private PlayerState currentState = PlayerState.idle;

    private void Awake() // �ʱ�ȭ
    {
        Instance = this;

        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponentInChildren<Collider>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    } // Awake ed

    private void Update() // ���� ����
    {
        if (currentState == PlayerState.Dead) return;

        if (GameManager.Instance.GameOver && currentState != PlayerState.Dead)
        {
            ChangeState(PlayerState.Dead);
            playerCollider.enabled = false;
            Destroy(this);
            return;
        } // ���ӿ��� ó��

        HandleInput();
        HandleState();

        // �޺� Ÿ�̸� ����
        if (currentCombo > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > comboResetTime)
            {
                currentCombo = 0; // ���� �ð� ������ �޺� �ʱ�ȭ
            }
        }
    } // Update ed

    void HandleInput() // �÷��̾� ���� ����
    {
        if (currentState == PlayerState.Dead) return; // ���� ���� �� ���� �Ұ�

        // �̵� ���� ����(����Ű)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(h, 0, v).normalized;

        // ���� ����
        if (Input.GetMouseButtonDown(0) && currentState != PlayerState.Atk && currentState != PlayerState.Dash)
        { StartComboAtk(); }

        // ��� ����
        if (Input.GetKeyDown(KeyCode.LeftShift) && moveInput.magnitude > 0.1f && currentState == PlayerState.Move)
        {ChangeState(PlayerState.Dash);}

        // ȸ�� �Ҹ��� ���
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {Inventory.Instance.UseItem(ItemType.Heal);}

        // ���� �Ҹ��� ���
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {Inventory.Instance.UseItem(ItemType.Shield);}
    } // HandleInput ed

    void HandleState() // ���º� �޼��� ó��
    {
        switch (currentState)
        {
            case PlayerState.idle:
                if (moveInput.magnitude > 0.1f) ChangeState(PlayerState.Move); // ����Ű �Է½� �̵� ���� ��ȯ
                break;
            case PlayerState.Move:
                HandleMovement(); // �̵� ����
                break;
            case PlayerState.Atk:
                // �ִϸ��̼� �̺�Ʈ�� ó��
                break;
            case PlayerState.Dash:
                HandleDash(); // �뽬 ����
                break;
        } // switch ed
    } // HandleState ed

    void ChangeState(PlayerState newState) // ���º� �ִ� ��ȯ �޼���
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
                animator.Play($"Player_Atk{currentCombo}"); // �ܰ躰 �ִ� ����
                break;
            case PlayerState.Dead:
                animator.Play("Player_die");
                break;
        } // switch ed
    } // ChangeState ed

    void HandleMovement() // �̵� �޼���
    {
        if (moveInput.magnitude <= 0.1f)
        {
            ChangeState(PlayerState.idle);
            return;
        }
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.deltaTime); // �̵�(rb���)
        // ���� ��ȯ
        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x > 0; // ��������Ʈ ��ȯ
            // ��Ʈ�ڽ� ���� ��ȯ
            Vector3 pos = hitBox.localPosition;
            pos.x = Mathf.Abs(pos.x) * (moveInput.x < 0 ? -1 : 1);
            hitBox.localPosition = pos;
        } // if ed
    } // HandleMovement ed

    void HandleDash() // �뽬 �޼���
    {
        if (dashTimer == 0f)
        {
            dashDirection = moveInput.normalized;
            animator.SetTrigger("Dash");

            // �뽬 ���� �� �ݶ��̴� ��Ȱ��ȭ
            Collider col = GetComponent<Collider>();
            if (col != null)
                col.enabled = false;
        }

        dashTimer += Time.deltaTime;
        rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.deltaTime);

        if (dashTimer >= dashDuration)
        {
            dashTimer = 0f;

            // �뽬 ���� �� �ݶ��̴� Ȱ��ȭ
            Collider col = GetComponent<Collider>();
            if (col != null)
                col.enabled = true;

            ChangeState(PlayerState.idle);
        }
    }

    // ���� ó�� �޼���(�޺�)
    void StartComboAtk()
    {
        if (currentCombo < maxComboLevel) // ���� �޺�ī��Ʈ�� �޺��������� ���� ��
        {
            currentCombo++; // �޺�ī��Ʈ +1
            comboTimer = 0f; // �޺�Ÿ�̸� �ʱ�ȭ
            ChangeState(PlayerState.Atk); // ���� ��ȯ
        } else
        {
            currentCombo = 1; // �޺� ����
            comboTimer = 0f; // �޺�Ÿ�̸� �ʱ�ȭ
            ChangeState(PlayerState.Atk);
        }
    } // StartComboAtk ed

    // ���� �ݶ��̴� ����
    public void ActiveSkillCollider(int idx) => skillColliders[idx].SetActive(true);
    public void InactiveSkillCollider(int idx) => skillColliders[idx].SetActive(false);
    public void CanMove() {if (currentState == PlayerState.Atk) ChangeState(PlayerState.idle);}

    // Ŀ�� �����
    private void OnApplicationFocus(bool focus){Cursor.lockState = focus ? CursorLockMode.Locked : CursorLockMode.None;}

} // class ed
