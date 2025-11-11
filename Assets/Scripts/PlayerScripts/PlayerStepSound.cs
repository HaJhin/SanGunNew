using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class PlayerStepSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip stepSound;
    public Player player;

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
            if (player.CurrentState == Player.PlayerState.Move)
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
