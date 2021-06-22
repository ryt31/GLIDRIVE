using System.Collections;
using System.Collections.Generic;
using Glidrive.Util;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VRFade : MonoBehaviour
{
    private static VRFade instance;
    public static VRFade Instance => instance;

    [SerializeField] private Renderer fade;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void BlackOut(float time)
    {
        StartCoroutine(BlackOutRoutine(time));
    }
    private IEnumerator BlackOutRoutine(float time)
    {
        var elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            fade.material.SetFloat("Vector1_438D9144",elapsedTime / time);
            yield return null;
        }

        fade.material.SetFloat("Vector1_438D9144", 1.0f);
    }

    public void BlackIn(float time)
    {
        StartCoroutine(BlackInRoutine(time));
    }
    private IEnumerator BlackInRoutine(float time)
    {
        var elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            fade.material.SetFloat("Vector1_438D9144",1.0f - (elapsedTime / time));
            yield return null;
        }

        fade.material.SetFloat("Vector1_438D9144", 0.0f);
    }
}
