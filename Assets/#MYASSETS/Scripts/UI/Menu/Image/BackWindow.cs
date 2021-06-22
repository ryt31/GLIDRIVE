using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Glidrive.Util;

public class BackWindow : MonoBehaviour
{
    private Image image;
    private const float FADE_OUT_TIME = 1.0f;
    private const float FADE_IN_TIME = 1.0f;
    
    private void Start()
    {
        image = GetComponent<Image>();
    }
    
    private IEnumerator FadeOutRoutine(float time)
    {
        var elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            var color = image.color;
            image.color = new Color(color.r, color.g, color.b, Util.Map(elapsedTime, 0.0f, 1.0f, 1.0f, 0.0f));
            yield return null;
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine(FADE_OUT_TIME));
    }

    private IEnumerator FadeInRoutine(float time)
    {
        var elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            var color = image.color;
            image.color = new Color(color.r, color.g, color.b, Util.Map(elapsedTime, 0.0f, 1.0f, 0.0f, 1.0f));
            yield return null;
        }
    }
    
    public void FadeIn()
    {
        StartCoroutine(FadeInRoutine(FADE_IN_TIME));
    }
}
