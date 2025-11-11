using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public SceneFadeController fadeController;
    public GameObject pauseUI; // 일시정지 UI
    public GameObject optionUI; // 옵션 UI
    private bool isPaused = false;

    private void Awake()
    {
        pauseUI.SetActive(false);
        optionUI.SetActive(false);
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
        optionUI.SetActive(false); // 옵션창 비활성화
        GameManager.Instance.SetPause(isPaused);
    }

    public void OptionBtn()
    {
        pauseUI.SetActive(false);
        optionUI.SetActive(true);
    }

    public void GoTitleBtn()
    {
        StartCoroutine(GoTitle());
    }

    public IEnumerator GoTitle()
    {
        yield return StartCoroutine(fadeController.FadeOut());
        SaveManager.ClearSave();
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start");
    }
}
