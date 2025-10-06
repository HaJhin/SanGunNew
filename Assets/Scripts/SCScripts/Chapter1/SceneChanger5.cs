using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger5 : MonoBehaviour
{
    public SceneFadeController fadeController;
    private bool isChanging = false;
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
        SceneManager.LoadScene("S2_1"); // �� ��ȯ
    }
}
