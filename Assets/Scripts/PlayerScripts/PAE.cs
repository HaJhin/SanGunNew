using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAE : MonoBehaviour
{
    public PlayerCon player;
    public GameObject[] skillColliders;

    public void ActiveSkillCollider(int idx)
    {
        player.ActiveSkillCollider(idx); // 부모의 메서드 실행
    }

    public void InactiveSkillCollider(int idx)
    {
        player.InactiveSkillCollider(idx); // 부모의 메서드 실행
    }

    public void CanMove()
    {
        player.Canmove();
    }
}
