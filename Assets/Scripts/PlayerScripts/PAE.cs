using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAE : MonoBehaviour
{
    public PlayerCon player;
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
        player.Canmove();
    }
}
