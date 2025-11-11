using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slash : MonoBehaviour
{
    public Image slashImage;          // 참격 이미지
    public float fadeInDuration = 0.1f;
    public float fadeOutDuration = 0.3f;
    public float moveDistance = 100f; // 스윽 이동 거리 (UI 공간에서)
    public float stayTime = 0.1f;     // 잠시 멈추는 시간

    private Vector3 startPos;

    void Awake()
    {
        if (slashImage == null)
            slashImage = GetComponent<Image>();
        startPos = slashImage.rectTransform.localPosition;
        slashImage.color = new Color(1, 1, 1, 0);
    }

    public void PlaySlashEffect()
    {
        StopAllCoroutines();
        StartCoroutine(SlashRoutine());
    }

    IEnumerator SlashRoutine()
    {
        // 초기화
        slashImage.rectTransform.localPosition = startPos;

        // Fade in
        float t = 0;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            float a = t / fadeInDuration;
            slashImage.color = new Color(1, 1, 1, a);
            yield return null;
        }

        // 이동 + 잠깐 유지
        float moveT = 0;
        while (moveT < stayTime)
        {
            moveT += Time.deltaTime;
            slashImage.rectTransform.localPosition = startPos + Vector3.right * (moveDistance * moveT);
            yield return null;
        }

        // Fade out
        t = 0;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            float a = 1 - (t / fadeOutDuration);
            slashImage.color = new Color(1, 1, 1, a);
            yield return null;
        }

        // 초기 상태 복귀
        slashImage.color = new Color(1, 1, 1, 0);
        slashImage.rectTransform.localPosition = startPos;
    }
}
