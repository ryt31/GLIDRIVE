using System.Collections;
using System.Collections.Generic;
using Glidrive.Util;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class YesStageSelectButton : BaseButtonTransitioner
{
    private const float FADE_OUT_TIME = 1.0f;
    private const float FADE_IN_TIME = 1.0f;

    [SerializeField] private NoStageSelectButton noButton;
    [SerializeField] private AreYouReadyImage areYouReadyButton;
    [SerializeField] private List<BackWindowSmallSelectImage> backWindows;
    private Image image;
    private Color initColor;
    private bool isTransparent = true;
    private MenuController menuController;
    private YesStageSelectAudio audio;

    private void Start()
    {
        image = GetComponent<Image>();
        var c = image.color;
        initColor = new Color(c.r,c.g,c.b,1.0f);
        menuController = gameObject.transform.root.GetComponent<MenuController>();
        audio = GetComponent<YesStageSelectAudio>();

        menuController.MenuType
            .Where(mType => mType == MenuType.YesNoGameStart && isTransparent)
            .Subscribe(_ => { StartCoroutine(ButtonFadeInRoutine(FADE_IN_TIME)); });
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        image.color = downColor;
        audio.ShotAudio();
        if (!isTransparent)
        {
            FadeOut(true);
            noButton.FadeOut(false);
            areYouReadyButton.FadeOut();
            foreach (var b in backWindows) b.FadeOut();
        }
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

    private IEnumerator ButtonFadeOutRoutine(float time, bool isYes)
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
        image.raycastTarget = false;
        if (isYes) menuController.MenuType.Value = MenuType.GameStart;
    }

    public void FadeOut(bool isYes)
    {
        StartCoroutine(ButtonFadeOutRoutine(FADE_OUT_TIME, isYes));
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