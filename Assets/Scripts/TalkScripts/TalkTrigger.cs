using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTrigger : MonoBehaviour
{
    public TalkManager talkManager;
    public string talkID; // ����� ��ũ�α�
    public bool hasTalked = false; // ��ȭ ���� ����
    public bool isTalking = false; // ��ȭ �� ����

    public string Flag; // �÷���

    private void Awake()
    {
        if (talkManager == null)
        {
            talkManager = FindObjectOfType<TalkManager>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTalked && other.CompareTag("PlayerHitbox"))
        {
            Debug.Log("��ȭ ����");
            hasTalked = true;
            Debug.Log("��ȭ ����!");
            StartTalkSequence();
        }
    } // OnTriggerEnter ed

    void StartTalkSequence()
    {
        if (talkManager != null && !isTalking)
        {
            isTalking = true;
            GameManager.Instance.SetPause(isTalking);
            talkManager.StartTalk(talkID, OnTalkEnd);
        }
    } // StartTalkSequence ed

    void OnTalkEnd()
    {
        Debug.Log("��ȭ ����");
        isTalking = false;
        GameManager.Instance.SetPause(isTalking);
        if (Flag != null)
        {
            FlagManager.Instance.SetFlag(Flag, true);
        }
    } // OnTalkEnd
}
