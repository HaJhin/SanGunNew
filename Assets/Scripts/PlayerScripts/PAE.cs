using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAE : MonoBehaviour
{
    // Player Attack Event = PAE

    // public PlayerCon player;
    public Player player;
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
        player.CanMove(); // 부모의 메서드 실행
    }

    public void ReturnIdle()
    {
        player.ReturnIdle(); // 부모의 메서드 실행
    }

    public void DashZero()
    {
        player.DashZero(); // 부모의 메서드 실행
    }
}
