using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance; // �̱���
    public HUDUI HUDUI;

    [Header("������")]
    public int healAmount = 0; // ȸ�� ������ ����
    public int shieldAmount = 0; // ���� ������ ����
    

    private void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject); 
    } // Awake ed

    private void Start()
    {
        HUDUI.UpdateItem1(healAmount);
        HUDUI.UpdateItem2(shieldAmount);
    } // Start ed

    public void AddItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Heal:
                healAmount++;
                HUDUI.UpdateItem1(healAmount);
                break;
            case ItemType.Shield: 
                shieldAmount++;
                HUDUI.UpdateItem2(shieldAmount);
                break;
        } // switch ed
    } // AddItem ed

    public void UseItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Heal:
                if (healAmount != 0 && !GameManager.Instance.IsFull()) 
                { 
                    Debug.Log("ü�� ȸ��!");
                    GameManager.Instance.Heal(1);
                    healAmount--;
                    HUDUI.UpdateItem1(healAmount);
                } else { Debug.Log("ȸ�� X"); return; }
                    break;

            case ItemType.Shield:
                if (shieldAmount != 0 && !GameManager.Instance.ShieldIsFull())
                {
                    Debug.Log("���� ����.");
                    GameManager.Instance.ShieldCharge(1);
                    shieldAmount--;
                    HUDUI.UpdateItem2(shieldAmount);
                } else { Debug.Log("���� ������ ����"); return; }
                    break;
        } // switch ed

    } // UseItem ed

} // class ed
