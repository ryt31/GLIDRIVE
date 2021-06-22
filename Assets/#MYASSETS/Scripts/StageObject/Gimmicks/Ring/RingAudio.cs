using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingAudio : MonoBehaviour
{
    private AudioSource ringAudioSource;
    private AudioSource accelAudioSource;
    private AudioClip ringSe;
    private AudioClip accelSe;
    private AudioSource[] audios;

    private void Start()
    {
        audios = GetComponents<AudioSource>();
        ringAudioSource = audios[0];
        accelAudioSource = audios[1];
        ringSe = ringAudioSource.clip;
        accelSe = accelAudioSource.clip;
    }

    public void ShotAudio()
    {
        ringAudioSource.PlayOneShot(ringSe);
        accelAudioSource.PlayOneShot(accelSe);
    }
}
