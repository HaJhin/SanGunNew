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
    public CanvasGroup fadeOverlay; // ȭ�� ��ü�� ���� ���� �г�
    public float fadeOutDuration = 1.5f;
    public string titleSceneName = "Title"; // Ÿ��Ʋ �� �̸�

    private bool canProceed = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // �ؽ�Ʈ ó������ ��� ����
        foreach (var t in texts)
        {
            Color c = t.color;
            c.a = 0f;
            t.color = c;
        }

        // ���� �г� �ʱ�ȭ
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

        // ��� �ؽ�Ʈ�� ��Ÿ�� ��, �����̽��� �Է� ����
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
        // Ÿ��Ʋ ������ �̵�
        SceneManager.LoadScene(titleSceneName);
    }
}