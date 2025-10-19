using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTrigger : MonoBehaviour
{
    public TalkManager talkManager;
    public string talkID;
    public bool isPlayerInRange = false;


    private void Awake()
    {
        if (talkManager == null)
        {
            talkManager = FindObjectOfType<TalkManager>();
        }
    }
    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (talkManager != null)
            {
                Time.timeScale = 0f; // 시간 정지
                talkManager.StartTalk(talkID,OnTalkEnd); // 
            } // if ed
        } // if ed
    } // Update ed

    void OnTalkEnd()
    {
        Time.timeScale = 1f; // 시간은 다시 움직인다
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHitbox"))
            Debug.Log("대화 가능");
            isPlayerInRange = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHitbox"))
            Debug.Log("대화가능상태 해제");
            isPlayerInRange = false;
    } 
}
