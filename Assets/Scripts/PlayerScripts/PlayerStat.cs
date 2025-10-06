using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{   
    GameManager gameManager;

    public int MaxHP = 5; // 최대체력
    public int HP; // 현재 체력
    public int Atk = 5; // 공격력
    public int Gold = 0; // 현재 재화 보유량

    private void Start()
    {
        
    }

}
