using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    [SerializeField] private TalkLog2 talkLog;
    [SerializeField] private GameObject dialoguePanel; // 대화창
    [SerializeField] private Text speakerText; // 화자
    [SerializeField] private Text dialogueText; // 내용

    private TalkLogSO currentTalk;
    private bool isTalking = false;
    private Action onTalkEndCallback; // 대화 종료 콜백

    public void Awake()
    {
        dialoguePanel.SetActive(false);
    }

    // 특정 ID에서 대화 시작
    public void StartTalk(string StartID,Action onTalkEnd = null) // 대화 시작
    {
        currentTalk = FindTalk(StartID);
        if (currentTalk != null)
        {
            isTalking = true;
            onTalkEndCallback = onTalkEnd; // 콜백 저장
            dialoguePanel.SetActive(true); // 대화창 열기
            ShowDialogue();
        }
    }

    private void Update()
    {
        if (isTalking && Input.GetKeyUp(KeyCode.Space)) NextTalk();
     } // Update ed

    private void ShowDialogue() // 대사 스크립트 띄우기
    {
        if (currentTalk == null) return;

        speakerText.text = currentTalk.Speaker;
        dialogueText.text = currentTalk.Dialogue;
    } // ShowDialogue ed

    private void NextTalk() // 다음 대화로 넘어가기 *없을 시 대화 종료
    {
        if(currentTalk.NextID == "None" || string.IsNullOrEmpty(currentTalk.NextID))
        {
            EndTalk();
            return;
        } // if ed 
        currentTalk = FindTalk(currentTalk.NextID);
        ShowDialogue();
    } // NextTalk ed

    private void EndTalk() // 대화 종료
    {
        Debug.Log("대화 종료");
        isTalking = false;
        dialoguePanel.SetActive(false); // 대화창 닫기
        currentTalk = null;

        // 콜백 실행
        onTalkEndCallback?.Invoke();
        onTalkEndCallback = null;
    } // EndTalk ed

    private TalkLogSO FindTalk(string id) // 대화 스크립트 찾기
    {
        for (int i = 0; i < talkLog.Story.Count; i++)
        {
            if (talkLog.Story[i].ID == id) return talkLog.Story[i];
        } return null;
    } // FindTalk ed

} // class ed
