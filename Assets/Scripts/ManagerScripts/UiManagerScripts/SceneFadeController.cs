using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        CustomStart();
    }

    private void CustomStart() { StartCoroutine(FadeIn()); }

    public IEnumerator FadeIn()
    {
        yield return Fade(1f, 0f);
    }

    public IEnumerator FadeOut()
    {
        yield return Fade(0f, 1f);
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}


