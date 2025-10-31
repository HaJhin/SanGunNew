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
            shopUI.SetActive(isOpen); // 시작할 때는 닫혀있음
    }

    public void ShowItemInfo(string name,int price,int ID) // 쇼 아이템 인포
    {
        currentItem = name;
        currentPrice = price;
        currentID = ID;
        Debug.Log(currentItem + currentPrice + currentID);
        itemName.text = name;
        itemPrice.text = price.ToString();
    }

    public void OnBuyBtnClicked() // 구매버튼 클릭
    {
        int playerGold = GameManager.Instance.Gold;

        if (playerGold >= currentPrice)
        {
            GameManager.Instance.SpendGold(currentPrice);
            ApplyItem(currentID);
            Debug.Log($"{currentItem} 구매완료");
        } else
        {
            Debug.Log("골드 부족. 구매 실패...");
        }
    } // OnBuyBtnClicked ed 

    private void ApplyItem(int ID) // 아이템 추가
    {
        switch (ID) 
        {
            case 1:
                if (GameManager.Instance.maxComboCount < 3) 
                {
                    GameManager.Instance.maxComboCount += 1;
                    GameManager.Instance.atk += 3;

                } else { Debug.Log("콤보 레벨 최대!");  return; }
                break;
            case 2:
                if (!GameManager.Instance.dashAtk)
                {
                    GameManager.Instance.dashAtk = true;
                }
                else { Debug.Log("대시어택 이미 활성화!"); return; }
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
        GameManager.Instance.SetPause(true); // 시간 정지 요청
    }
    public void CloseShop()
    {
        shopUI.SetActive(false);
        isOpen = false;
        GameManager.Instance.SetPause(false); // 시간 재개 요청
    }
} // class ed
