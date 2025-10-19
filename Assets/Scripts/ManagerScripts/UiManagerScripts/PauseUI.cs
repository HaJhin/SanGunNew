using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public GameObject pauseUI; // 일시정지 UI
    private bool isPaused = false;

    private void Awake()
    {
        pauseUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) { Resume(); } else { Pause(); }
        }
    } // Update ed

    void Pause()
    {
        pauseUI.SetActive(true); // UI 활성화
        Time.timeScale = 0f; // 시간 정지
        Cursor.visible = true; // 커서 표시
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
    }

    void Resume()
    {
        pauseUI.SetActive(false); // UI 비활성화
        Time.timeScale = 1.0f; // 시간 복구
        Cursor.visible = false; // 커서 숨김
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }
}
