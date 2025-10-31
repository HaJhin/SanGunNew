using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    [SerializeField] private TalkLog2 talkLog;
    [SerializeField] private GameObject dialoguePanel; // ��ȭâ
    [SerializeField] private Text speakerText; // ȭ��
    [SerializeField] private Text dialogueText; // ����

    private TalkLogSO currentTalk;
    private bool isTalking = false;
    private Action onTalkEndCallback; // ��ȭ ���� �ݹ�

    public void Awake()
    {
        dialoguePanel.SetActive(false);
    }

    // Ư�� ID���� ��ȭ ����
    public void StartTalk(string StartID,Action onTalkEnd = null) // ��ȭ ����
    {
        currentTalk = FindTalk(StartID);
        if (currentTalk != null)
        {
            isTalking = true;
            onTalkEndCallback = onTalkEnd; // �ݹ� ����
            dialoguePanel.SetActive(true); // ��ȭâ ����
            ShowDialogue();
        }
    }

    private void Update()
    {
        if (isTalking && Input.GetKeyUp(KeyCode.Space)) NextTalk();
     } // Update ed

    private void ShowDialogue() // ��� ��ũ��Ʈ ����
    {
        if (currentTalk == null) return;

        speakerText.text = currentTalk.Speaker;
        dialogueText.text = currentTalk.Dialogue;
    } // ShowDialogue ed

    private void NextTalk() // ���� ��ȭ�� �Ѿ�� *���� �� ��ȭ ����
    {
        if(currentTalk.NextID == "None" || string.IsNullOrEmpty(currentTalk.NextID))
        {
            EndTalk();
            return;
        } // if ed 
        currentTalk = FindTalk(currentTalk.NextID);
        ShowDialogue();
    } // NextTalk ed

    private void EndTalk() // ��ȭ ����
    {
        Debug.Log("��ȭ ����");
        isTalking = false;
        dialoguePanel.SetActive(false); // ��ȭâ �ݱ�
        currentTalk = null;

        // �ݹ� ����
        onTalkEndCallback?.Invoke();
        onTalkEndCallback = null;
    } // EndTalk ed

    private TalkLogSO FindTalk(string id) // ��ȭ ��ũ��Ʈ ã��
    {
        for (int i = 0; i < talkLog.Story.Count; i++)
        {
            if (talkLog.Story[i].ID == id) return talkLog.Story[i];
        } return null;
    } // FindTalk ed

} // class ed
