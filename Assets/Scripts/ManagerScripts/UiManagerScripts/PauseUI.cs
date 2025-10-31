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
        isPaused = true;
        pauseUI.SetActive(true); // UI 활성화
        GameManager.Instance.SetPause(isPaused);
    }

    void Resume()
    {
        isPaused = false;
        pauseUI.SetActive(false); // UI 비활성화
        GameManager.Instance.SetPause(isPaused);
    }
}
