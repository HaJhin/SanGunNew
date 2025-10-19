using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public GameObject pauseUI; // �Ͻ����� UI
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
        pauseUI.SetActive(true); // UI Ȱ��ȭ
        Time.timeScale = 0f; // �ð� ����
        Cursor.visible = true; // Ŀ�� ǥ��
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
    }

    void Resume()
    {
        pauseUI.SetActive(false); // UI ��Ȱ��ȭ
        Time.timeScale = 1.0f; // �ð� ����
        Cursor.visible = false; // Ŀ�� ����
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }
}
