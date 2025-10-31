using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public HUDUI HUDUI;
    public HPUI HPUI;
    public ShieldUI ShieldUI;

    public bool pauseNow = false;

    public bool GameOver = false;

    [Header("stats")]
    public int MaxHP = 5;
    public int HP;
    public int maxShield = 5;
    public int shield;
    public int atk = 5;
    public int maxComboCount = 1;
    public bool dashAtk = false;

    [Header("point")]
    public int Gold = 0;

    void Awake() // DDOL ó��
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    } // ed

    private void Start()
    {
        HP = MaxHP;
        shield = 0;
        // HPUI,ShieldUI �ʱ�ȭ
        HPUI.init(MaxHP);
        ShieldUI.Init(maxShield);
        HPUI.setHP(HP);
        ShieldUI.SetShield(shield);

        HUDUI.UpdateGold(Gold);
    } // Start ed

    public bool IsFull(){if (HP == MaxHP) return true; else return false;} // ü�� �ִ�ġ Ȯ��
    public bool ShieldIsFull() {if (shield == maxShield) return true; else return false;} // ���� �ִ�ġ Ȯ��

    public void TakeDamage(int dmg) // ü�� or ���� ����
    {
        DamageOverlay.instance.ShowHitEffect();
        Debug.Log("�걺 ������!");
        if (shield > 0)
        {
            shield = Mathf.Clamp(shield - dmg, 0, maxShield);
            ShieldUI.SetShield(shield);
        }
        else
        {
            HP = Mathf.Clamp(HP - dmg, 0, MaxHP);
            HPUI.setHP(HP);
            if (HP <= 0) GameOver = true;
        } // if - else ed
    } // TakeDamage ed

    public void Heal(int amount) // ü�� ȸ��
    {
        if (HP != MaxHP)
        {
            HP = Mathf.Clamp(HP + amount, 0, MaxHP);
            Debug.Log("ü��ȸ��! ���� ü�� : " + HP);
            HPUI.setHP(HP);
        } else { Debug.Log("ü���� �ִ�ġ�Դϴ�."); return; }
    } // Heal ed

    public void ShieldCharge(int amount) // ���� ����
    { 
        if (shield != maxShield)
        {
            shield = Mathf.Clamp(shield + amount, 0, maxShield);
            Debug.Log("���� ����! ���� ���� : " + shield);
            ShieldUI.SetShield(shield);
        } else { Debug.Log("���尡 �ִ�ġ�Դϴ�."); return; }
    } // ShieldCharge ed

    public void AddGold(int amount) // ��� ȹ��
    {
        Gold += amount;
        HUDUI.UpdateGold(Gold);
    } // AddGold ed

    public void SpendGold(int amount) // ��� �Һ�
    {
        Gold = Mathf.Max(0, Gold - amount);
        HUDUI.UpdateGold(Gold);
    } // SpendGold ed

    public void SetPause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseNow = true;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pauseNow = false;
        }
    } // SetPause ed

} // ed 
