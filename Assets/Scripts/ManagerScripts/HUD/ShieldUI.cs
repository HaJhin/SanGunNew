using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUI : MonoBehaviour
{
    public GameObject ShieldIcon;   // 쉴드 아이콘 프리팹

    private List<GameObject> shieldUIList = new List<GameObject>();
    private int currentShield;

    // 초기화 (최대 쉴드 아이콘 생성)
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

    // 쉴드 갱신
    public void SetShield(int newShield)
    {
        newShield = Mathf.Clamp(newShield, 0, shieldUIList.Count);
        currentShield = newShield;
        UpdateUI();
    }

    // 단순히 아이콘 on/off
    private void UpdateUI()
    {
        for (int i = 0; i < shieldUIList.Count; i++)
        {
            shieldUIList[i].SetActive(i < currentShield);
        }
    }
}
