using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ShopBtn : MonoBehaviour
{
    public int itemID;
    public string itemName;
    public int price;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnBtnClicked);
    } // Awake ed

    private void OnBtnClicked()
    {
        Shop.Instance.ShowItemInfo(itemName,price,itemID);
    }
}
