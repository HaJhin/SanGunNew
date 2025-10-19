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
                Time.timeScale = 0f; // �ð� ����
                talkManager.StartTalk(talkID,OnTalkEnd); // 
            } // if ed
        } // if ed
    } // Update ed

    void OnTalkEnd()
    {
        Time.timeScale = 1f; // �ð��� �ٽ� �����δ�
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHitbox"))
            Debug.Log("��ȭ ����");
            isPlayerInRange = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHitbox"))
            Debug.Log("��ȭ���ɻ��� ����");
            isPlayerInRange = false;
    } 
}
