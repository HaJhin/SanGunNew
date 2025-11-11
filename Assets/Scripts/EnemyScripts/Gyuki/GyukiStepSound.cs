using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyukiStepSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip stepSound;
    public Gyuki gyuki;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {

            audioSource.clip = stepSound;
        }
    }

    private void Update()
    {
        CheckPause();
        if (!GameManager.Instance.pauseNow)
        {
            if (gyuki.currentState == Gyuki.BossState.Move)
            {
                if (!audioSource.isPlaying) audioSource.Play();
            }
            else
            {
                if (audioSource.isPlaying) audioSource.Stop();
            }
        }
    } // Update ed

    private void CheckPause()
    {
        if (GameManager.Instance.pauseNow)
        {
            audioSource.Stop();
        }
    }
}
