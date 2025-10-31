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
}
