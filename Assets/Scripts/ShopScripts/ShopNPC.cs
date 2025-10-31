using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    public GameObject canBuy;
    public bool isPlayerInRange = false;

    private void Start()
    {
        if (canBuy != null)
            canBuy.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyUp(KeyCode.E))
        {
            Shop.Instance.ToggleShop();
        } // if ed
    } // Update ed

    private void OnTriggerEnter(Collider other) // 상점 활성화 인식
    {
        if (other.CompareTag("PlayerHitbox"))
        {
            isPlayerInRange = true;
            canBuy.SetActive(isPlayerInRange);
            Debug.Log("상점 이용 가능");
        }
    } // OTEnter ed

    private void OnTriggerExit(Collider other) // 상점 비활성화 인식
    {
        if (other.CompareTag("PlayerHitbox"))
        {
            isPlayerInRange = false;
            canBuy.SetActive(isPlayerInRange);
            Debug.Log("상점 범위 이탈");
        }
    } // OTExit ed
}
