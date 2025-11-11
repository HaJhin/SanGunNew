using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public AudioSource audioSource;
    private string currentSceneName = "";

    [Header("기본 BGM (특수 스테이지 외 모든 곳에서 재생)")]
    public AudioClip defaultBGM;

    [Header("특수 스테이지 BGM 리스트")]
    public List<SceneBGM> bgmList = new List<SceneBGM>();

    private Dictionary<string,AudioClip> bgmDict = new Dictionary<string,AudioClip>(); // 내부 검색 딕셔너리

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); // AudioSource 가져오기

        // 리스트의 데이터를 딕셔너리로 변환 (빠른 검색용)
        foreach (var item in bgmList)
        {
            if (!bgmDict.ContainsKey(item.sceneName))
                bgmDict.Add(item.sceneName, item.bgmClip);
        }
        // 첫 씬 즉시 재생
        currentSceneName = SceneManager.GetActiveScene().name;
        PlayBGMForScene(currentSceneName);
    } // Awake ed

    private void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name; // 씬 이름 체크

        if (sceneName !=  currentSceneName)
        {
            currentSceneName = sceneName;
            PlayBGMForScene(sceneName); // 새 씬의 BGM 재생
        }
    } // Update ed

    public void PlayBGMForScene(string sceneName)
    {
        // 특수 스테이지라면 해당 BGM 재생
        if (bgmDict.TryGetValue(sceneName, out AudioClip specialClip))
        {
            if (audioSource.clip != specialClip)
            {
                audioSource.clip = specialClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            // 특수 스테이지가 아닐 경우, 기본 BGM 재생 (이미 재생 중이면 그대로 둠)
            if (audioSource.clip != defaultBGM)
            {
                audioSource.clip = defaultBGM;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    } // PlayBGMForScene ed

} // class ed

[System.Serializable]
public class SceneBGM
{
    public string sceneName; // 씬 이름 (예: "MainStage", "BossStage", "Ending")
    public AudioClip bgmClip; // 해당 씬에서 재생할 오디오 클립
} // SceneBGM ed
