using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumePrefs : MonoBehaviour
{
    [Header("UI Components")]
    public Slider bgmSlider;
    public Text bgmText;
    public Slider sfxSlider;
    public Text sfxText;

    [Header("Audio Mixer Reference")]
    public AudioMixer masterMixer;

    private const string BGM_KEY = "BGMVolume";
    private const string SFX_KEY = "SFXVolume";

    private void Awake()
    {
        // 저장된 볼륨 불러오기
        float bgmValue = PlayerPrefs.GetFloat(BGM_KEY, 0.5f);
        float sfxValue = PlayerPrefs.GetFloat(SFX_KEY, 0.5f);

        // 슬라이더 값
        bgmSlider.value = bgmValue;
        sfxSlider.value = sfxValue;

        // AudioMixer 연결
        SetBGMVolume(bgmValue);
        SetSFXVolume(sfxValue);
    } // Awake ed

    private void Start()
    {
        
    }

    private void OnBGMValueChanged(float value)
    {
        SetBGMVolume(value);
        PlayerPrefs.SetFloat(BGM_KEY, value);
    }

    private void OnSFXValueChanged(float value)
    {
        SetSFXVolume(value);
        PlayerPrefs.SetFloat(SFX_KEY, value);
    }

    private void SetBGMVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        masterMixer.SetFloat("BGMVolume", dB);   // AudioMixer 파라미터명
        bgmText.text = (value * 100f).ToString("F0");
    }

    private void SetSFXVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        masterMixer.SetFloat("SFXVolume", dB);
        sfxText.text = (value * 100f).ToString("F0");
    }
}
