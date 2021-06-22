using System.Collections;
using System.Collections.Generic;
using Glidrive.Manager;
using Glidrive.Util;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class YesButton : BaseButtonTransitioner
{
    private Image image;
    private Color initColor;
    private MenuController menuController;
    private YesAudio audio;
    private bool isTransparent = true;
    private const float FADE_OUT_TIME = 1.0f;
    private const float FADE_IN_TIME = 1.0f;

    private void Start()
    {
        image = GetComponent<Image>();
        var c = image.color;
        initColor = new Color(c.r,c.g,c.b,1.0f);
        menuController = gameObject.transform.root.GetComponent<MenuController>();
        audio = GetComponent<YesAudio>();

        menuController.MenuType
            .Where(mType => mType == MenuType.YesNoExit && isTransparent)
            .Subscribe(_ =>
            {
                StartCoroutine(ButtonFadeInRoutine(FADE_IN_TIME));
            });
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        image.color = downColor;
        audio.ShotAudio();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        image.color = downColor;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        image.color = hoverColor;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        image.color = initColor;
    }

    private IEnumerator ButtonFadeOutRoutine(float time)
    {
        image.raycastTarget = false;
        var elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            var color = image.color;
            image.color = new Color(color.r, color.g, color.b, Util.Map(elapsedTime, 0.0f, 1.0f, 1.0f, 0.0f));
            yield return null;
        }
        isTransparent = true;
    }

    public void FadeOut()
    {
        StartCoroutine(ButtonFadeOutRoutine(FADE_OUT_TIME));
    }
    
    private IEnumerator ButtonFadeInRoutine(float time)
    {
        var elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            var color = image.color;
            image.color = new Color(color.r, color.g, color.b, Util.Map(elapsedTime, 0.0f, 1.0f, 0.0f, 1.0f));
            yield return null;
        }

        isTransparent = false;
        image.raycastTarget = true;
    }
}