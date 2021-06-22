using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YesAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip se;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        se = audioSource.clip;
    }

    public void ShotAudio()
    {
        audioSource.PlayOneShot(se);
    }
}
