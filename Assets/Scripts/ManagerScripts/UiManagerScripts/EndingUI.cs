using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndingUI : MonoBehaviour
{
    public TMP_Text[] texts;
    public float fadeDuration = 2f;
    public float delayBetween = 1f;

    [Header("Fade Out to Title")]
    public CanvasGroup fadeOverlay; // 화면 전체를 덮는 검은 패널
    public float fadeOutDuration = 1.5f;
    public string titleSceneName = "Title"; // 타이틀 씬 이름

    private bool canProceed = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 텍스트 처음에는 모두 투명
        foreach (var t in texts)
        {
            Color c = t.color;
            c.a = 0f;
            t.color = c;
        }

        // 암전 패널 초기화
        if (fadeOverlay != null)
        {
            fadeOverlay.alpha = 0f;
        }

        StartCoroutine(FadeTexts());
    }

    void Update()
    {
        if (canProceed && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadeOutAndLoadTitle());
        }
    }

    IEnumerator FadeTexts()
    {
        foreach (var t in texts)
        {
            float elapsed = 0f;
            Color c = t.color;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                c.a = Mathf.Clamp01(elapsed / fadeDuration);
                t.color = c;
                yield return null;
            }

            yield return new WaitForSeconds(delayBetween);
        }

        // 모든 텍스트가 나타난 후, 스페이스바 입력 가능
        canProceed = true;
    }

    IEnumerator FadeOutAndLoadTitle()
    {
        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            if (fadeOverlay != null)
                fadeOverlay.alpha = Mathf.Clamp01(elapsed / fadeOutDuration);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        // 타이틀 씬으로 이동
        SceneManager.LoadScene(titleSceneName);
    }
}