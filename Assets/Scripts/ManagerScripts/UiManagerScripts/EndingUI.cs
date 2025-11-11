using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndingUI : MonoBehaviour
{
    public CanvasGroup[] texts;
    public CanvasGroup[] images;
    private float fadeTime = 1f, holdtime = 3f;

    private bool End = false;

    public CanvasGroup fadeImage;

    void Start() => StartCoroutine(Play());

    IEnumerator Play()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i] != null) yield return Fade(texts[i], 0, 1);
            yield return new WaitForSeconds(0.5f);
            if (images[i] != null) yield return Fade(images[i], 0, 1);
            yield return new WaitForSeconds(holdtime);
            if (i != texts.Length - 1)
            {
                if (texts[i] != null) yield return Fade(texts[i], 1, 0);
                yield return new WaitForSeconds(0.5f);
                if (images[i] != null) yield return Fade(images[i], 1, 0);
                yield return new WaitForSeconds(0.5f);
            }
            End = true;
        }
        Debug.Log("엔딩 종료");
    } // Play ed

    IEnumerator Fade(CanvasGroup cg, float from, float to)
    {
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / fadeTime);
            yield return null;
        }
    }

    public void GoTitleBtn()
    {
        if (End && Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(GoTitle());
        }
    }

    private IEnumerator GoTitle()
    {
        if (fadeImage != null) yield return Fade(fadeImage, 0, 1);
        SaveManager.ClearSave();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Start");
    }
}