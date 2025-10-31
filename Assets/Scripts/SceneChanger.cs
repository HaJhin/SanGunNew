using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("���� UI")]
    public SceneFadeController fadeController;

    [Header("���� ��")]
    public string nextSceneName;

    [Header("����")]
    public string flagName = "Ending";

    private bool isChanging = false;

    private void Awake()
    {
        GameObject fadeImage = GameObject.FindGameObjectWithTag("Fade");
        if (fadeImage != null)
        {
            fadeController = fadeImage.GetComponent<SceneFadeController>();
        } else { Debug.LogWarning("fadeã�� ����."); }
        
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
        yield return fadeController.FadeOut(); // ���� ����
        SceneManager.LoadScene(nextSceneName); // �� ��ȯ
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
        yield return fadeController.FadeOut(); // ���� ����
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Ending"); // �� ��ȯ
    }
}
