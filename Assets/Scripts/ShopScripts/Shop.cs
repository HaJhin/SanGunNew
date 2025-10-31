using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop Instance;

    public GameObject shopUI;
    
    [Header("UI elements")]
    public Text itemName;
    public Text itemPrice;

    private int currentID;
    private string currentItem;
    private int currentPrice;

    private bool isOpen = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (shopUI != null)
            shopUI.SetActive(isOpen); // ������ ���� ��������
    }

    public void ShowItemInfo(string name,int price,int ID) // �� ������ ����
    {
        currentItem = name;
        currentPrice = price;
        currentID = ID;
        Debug.Log(currentItem + currentPrice + currentID);
        itemName.text = name;
        itemPrice.text = price.ToString();
    }

    public void OnBuyBtnClicked() // ���Ź�ư Ŭ��
    {
        int playerGold = GameManager.Instance.Gold;

        if (playerGold >= currentPrice)
        {
            GameManager.Instance.SpendGold(currentPrice);
            ApplyItem(currentID);
            Debug.Log($"{currentItem} ���ſϷ�");
        } else
        {
            Debug.Log("��� ����. ���� ����...");
        }
    } // OnBuyBtnClicked ed 

    private void ApplyItem(int ID) // ������ �߰�
    {
        switch (ID) 
        {
            case 1:
                if (GameManager.Instance.maxComboCount < 3) 
                {
                    GameManager.Instance.maxComboCount += 1;
                    GameManager.Instance.atk += 3;

                } else { Debug.Log("�޺� ���� �ִ�!");  return; }
                break;
            case 2:
                if (!GameManager.Instance.dashAtk)
                {
                    GameManager.Instance.dashAtk = true;
                }
                else { Debug.Log("��þ��� �̹� Ȱ��ȭ!"); return; }
                break;
            case 3:
                Inventory.Instance.AddItem(ItemType.Heal);
                break;
            case 4:
                Inventory.Instance.AddItem(ItemType.Shield);
                break;
        }
    }

    public void ToggleShop() {if (isOpen) CloseShop(); else OpenShop();}

    public void OpenShop()
    {
        shopUI.SetActive(true);
        isOpen = true;
        GameManager.Instance.SetPause(true); // �ð� ���� ��û
    }
    public void CloseShop()
    {
        shopUI.SetActive(false);
        isOpen = false;
        GameManager.Instance.SetPause(false); // �ð� �簳 ��û
    }
} // class ed
