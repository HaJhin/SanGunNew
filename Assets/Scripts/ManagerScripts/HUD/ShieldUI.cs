using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUI : MonoBehaviour
{
    public GameObject ShieldIcon;   // ���� ������ ������

    private List<GameObject> shieldUIList = new List<GameObject>();
    private int currentShield;

    // �ʱ�ȭ (�ִ� ���� ������ ����)
    public void Init(int maxShield)
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        shieldUIList.Clear();

        for (int i = 0; i < maxShield; i++)
        {
            GameObject iconGO = Instantiate(ShieldIcon, transform);
            shieldUIList.Add(iconGO);
        }
        currentShield = maxShield;
        UpdateUI();
    }

    // ���� ����
    public void SetShield(int newShield)
    {
        newShield = Mathf.Clamp(newShield, 0, shieldUIList.Count);
        currentShield = newShield;
        UpdateUI();
    }

    // �ܼ��� ������ on/off
    private void UpdateUI()
    {
        for (int i = 0; i < shieldUIList.Count; i++)
        {
            shieldUIList[i].SetActive(i < currentShield);
        }
    }
}
