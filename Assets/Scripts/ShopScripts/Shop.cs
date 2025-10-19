using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop Instance;

    public GameObject shopUI;
    public GameObject canBuy;
    public bool isPlayerInRange = false;

    [Header("UI elements")]
    public Text itemName;
    public Text itemPrice;

    private int currentID;
    private string currentItem;
    private int currentPrice;

    private void Awake()
    {
        Instance = this;
        // buyBtn.onClick.AddListener();
    }

    private void Start()
    {
        if (shopUI != null)
            shopUI.SetActive(false); // ������ ���� ��������
        if (canBuy != null)
            canBuy.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyUp(KeyCode.E))
        {
            ToggleShop();
        } // if ed
    } // Update ed

    private void ToggleShop() // ���� ���� Ű��
    {
        if (shopUI != null)
        {
            bool isActive = shopUI.activeSelf;
            shopUI.SetActive(!isActive);
            if (isActive ) 
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            } else 
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        } // if ed
    } // ToggleShop ed

    private void OnTriggerEnter(Collider other) // ���� Ȱ��ȭ �ν�
    {
        if (other.CompareTag("PlayerHitbox"))
        {
            isPlayerInRange = true;
            canBuy.SetActive(isPlayerInRange);
            Debug.Log("���� �̿� ����");
        }
    } // OTEnter ed

    private void OnTriggerExit(Collider other) // ���� ��Ȱ��ȭ �ν�
    {
        if (other.CompareTag("PlayerHitbox"))
        {
            isPlayerInRange = false;
            canBuy.SetActive(isPlayerInRange);
            Debug.Log("���� ���� ��Ż");
        }
    } // OTExit ed

    public void ShowItemInfo(string name,int price,int ID) // �� ������ ����
    {
        currentItem = name;
        currentPrice = price;
        currentID = ID;
        Debug.Log(currentItem + currentPrice + currentID);
        itemName.text = name;
        itemPrice.text = price.ToString();
    }

    public void OnBuyBtnClicked()
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

    private void ApplyItem(int ID)
    {
        switch (ID) 
        {
            case 1:
                if (Player.Instance.maxComboLevel < 3) 
                {
                    Player.Instance.maxComboLevel += 1; 
                } else { return; }
                break;
            case 2:
                if (!Player.Instance.DashAtk)
                {
                    Player.Instance.DashAtk = true;
                }
                else { return; }
                break;
            case 3:
                Inventory.Instance.AddItem(ItemType.Heal);
                break;
            case 4:
                Inventory.Instance.AddItem(ItemType.Shield);
                break;
        }
    }
} // class ed
