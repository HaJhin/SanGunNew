using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTrigger : MonoBehaviour
{
    public TalkManager talkManager;
    public string talkID; // 재생할 토크로그
    public bool hasTalked = false; // 대화 관람 여부
    public bool isTalking = false; // 대화 중 여부

    public string Flag; // 플래그

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
            Debug.Log("대화 시작");
            hasTalked = true;
            Debug.Log("대화 했음!");
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
        Debug.Log("대화 종료");
        isTalking = false;
        GameManager.Instance.SetPause(isTalking);
        if (Flag != null)
        {
            FlagManager.Instance.SetFlag(Flag, true);
        }
    } // OnTalkEnd
}
