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
        player.ActiveSkillCollider(idx); // �θ��� �޼��� ����
    }

    public void InactiveSkillCollider(int idx)
    {
        player.InactiveSkillCollider(idx); // �θ��� �޼��� ����
    }

    public void CanMove()
    {
        player.CanMove(); // �θ��� �޼��� ����
    }

    public void ReturnIdle()
    {
        player.ReturnIdle(); // �θ��� �޼��� ����
    }

    public void DashZero()
    {
        player.DashZero(); // �θ��� �޼��� ����
    }
}
