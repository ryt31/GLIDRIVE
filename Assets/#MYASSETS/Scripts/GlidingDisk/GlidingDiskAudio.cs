using System.Collections;
using System.Collections.Generic;
using Glidrive.GlidingDisk;
using UnityEngine;

public class GlidingDiskAudio : BaseGlidingDisk
{
    private AudioSource audio;
    private AudioClip se;
    
    protected override void OnInitialize()
    {
        audio = GetComponent<AudioSource>();
        se = audio.clip;
    }
    
    public void ShotAudio()
    {
        audio.PlayOneShot(se);
    }
}
