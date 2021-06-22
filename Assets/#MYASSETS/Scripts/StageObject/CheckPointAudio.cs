using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointAudio : MonoBehaviour
{
    private AudioSource audio;
    private AudioClip se;
    
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        se = audio.clip;
    }

    public void ShotAudio()
    {
        audio.PlayOneShot(se);
    }
}
