using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("암전 UI")]
    public SceneFadeController fadeController;

    [Header("다음 씬")]
    public string nextSceneName;

    [Header("엔딩")]
    public string flagName = "Ending";

    private bool isChanging = false;

    private void Awake()
    {
        GameObject fadeImage = GameObject.FindGameObjectWithTag("Fade");
        if (fadeImage != null)
        {
            fadeController = fadeImage.GetComponent<SceneFadeController>();
        } else { Debug.LogWarning("fade찾기 실패."); }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isChanging && other.CompareTag("Player"))
        {
            isChanging = true;
            StartCoroutine(ChangeScene());
        }
    }

    IEnumerator ChangeScene()
    {
        yield return fadeController.FadeOut(); // 암전 시작
        SceneManager.LoadScene(nextSceneName); // 씬 전환
    }

    private void OnEnable()
    {
        FlagManager.Instance.FlagChanged += OnFlagChanged;
    }

    private void OnDisable()
    {
        FlagManager.Instance.FlagChanged -= OnFlagChanged;
    }

    private void OnFlagChanged(string key, bool value)
    {
        if (key == flagName && value)
        {
            StartCoroutine(GoEnding());
        }
    } // OnFlagChanged ed

    IEnumerator GoEnding()
    {
        yield return fadeController.FadeOut(); // 암전 시작
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Ending"); // 씬 전환
    }
}
