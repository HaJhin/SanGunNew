using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverUI : MonoBehaviour
{
    [Header("¾ÏÀü UI")]
    public SceneFadeController fadeController;

    public GameObject gameoverUI;

    private bool isGameOverExecuted = false;

    private void Awake()
    {
        gameoverUI.SetActive(false);
    }

    private void Update()
    {
            if (GameManager.Instance.GameOver && !isGameOverExecuted)
            {
                Debug.Log("Game Over");
                StartCoroutine(GameOver());
                isGameOverExecuted = true;
            }
    }

    public void GoTitleBtn()
    {
        StartCoroutine(GoTitle());
    }


    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(fadeController.FadeOut());
        yield return new WaitForSeconds(0.5f);
        gameoverUI.SetActive(true);
        yield return StartCoroutine(fadeController.FadeIn());
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    } // GameOver ed

    public IEnumerator GoTitle()
    {
        yield return StartCoroutine(fadeController.FadeOut());
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Start");
    }
}