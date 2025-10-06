using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject shopUI;
    public bool isPlayerInRange = false;

    private void Start()
    {
        if (shopUI != null)
            shopUI.SetActive(false); // ������ ���� ��������
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyUp(KeyCode.E))
        {
            ToggleShop();
        } // if ed
    } // Update ed

    private void ToggleShop()
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("���� �̿� ����");
        }
    } // OTEnter ed

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("���� ���� ��Ż");
        }
    } // OTExit ed

} // class ed
