using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUI : MonoBehaviour
{
    public GameObject HPIcon;
    public float aniDelay = 0.1f;

    private List<HPIcon> hpUIList = new List<HPIcon>();
    private int currentHP;

    // 초기화(최대 체력 아이콘 생성)
    public void init(int MaxHP)
    {
        foreach(Transform child in transform)
            Destroy(child.gameObject);
        hpUIList.Clear();

        for (int i = 0; i < MaxHP; i++)
        {
            GameObject iconGO = Instantiate(HPIcon, transform);
            HPIcon icon = iconGO.GetComponent<HPIcon>();
            hpUIList.Add(icon);
        } // for ed
        currentHP = MaxHP;
    } // init ed

    public void setHP(int newHP)
    {
        newHP = Mathf.Clamp(newHP,0, hpUIList.Count);

        if (newHP < currentHP) // 데미지 → 오른쪽부터
            StartCoroutine(PlayDamageSequence(newHP));
        else if (newHP > currentHP) // 회복 → 왼쪽부터
            StartCoroutine(PlayRecoverSequence(newHP));

        currentHP = newHP;
    }

    private IEnumerator PlayDamageSequence(int targetHP)
    {
        for (int i = currentHP - 1; i >= targetHP; i--)
        {
            hpUIList[i].PlayDamage();
            yield return new WaitForSeconds(aniDelay);
        } // for ed 
    } // PDS ed

    private IEnumerator PlayRecoverSequence(int targetHP)
    {
        for (int i = currentHP; i < targetHP; i++)
        {
            hpUIList[i].PlayRecover();
            yield return new WaitForSeconds(aniDelay);
        } // for ed
    } // PRS ed

} // HPUI ed
