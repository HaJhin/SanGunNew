using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageOverlay : MonoBehaviour
{
    public static DamageOverlay instance;

    public Image overlayImage;
    public float fadeSpeed = 2f;
    public float maxAlpha = 0.5f;

    private Coroutine currentFade;

    private void Awake(){instance = this; overlayImage.color = new Color(overlayImage.color.r, overlayImage.color.g, overlayImage.color.b, 0);}

    public void ShowHitEffect()
    {
        if(currentFade != null) StopCoroutine(currentFade);
        currentFade = StartCoroutine(FadeEffect());
        
    } // ShowHitEffect ed

    private IEnumerator FadeEffect()
    {
        // 1. Alpha�� ������ �ø�
        float alpha = 0f;
        while (alpha < maxAlpha)
        {
            alpha += Time.deltaTime * fadeSpeed;
            SetAlpha(alpha);
            yield return null;
        }

        // 2. õõ�� �ٽ� �����ϰ�
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(0f);
        currentFade = null;
    }

    private void SetAlpha(float a)
    {
        Color c = overlayImage.color;
        c.a = Mathf.Clamp01(a);
        overlayImage.color = c;
    }
}
