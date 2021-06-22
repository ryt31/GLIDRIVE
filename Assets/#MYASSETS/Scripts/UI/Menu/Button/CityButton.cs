using System.Collections;
using System.Collections.Generic;
using Glidrive.Manager;
using Glidrive.Util;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CityButton : BaseButtonTransitioner
{
    private Image image;
    private Color initColor;
    private MenuController menuController;
    private CityAudio audio;
    private bool isTransparent = true;
    private const float FADE_OUT_TIME = 1.0f;
    private const float FADE_IN_TIME = 1.0f;
    
    // TODO 良くない...
    [SerializeField] private BackSepImage sep;
    [SerializeField] private BackButton backButton;
    [SerializeField] private List<LockedStageButton> lockedButtons;

    private void Start()
    {
        image = GetComponent<Image>();
        var c = image.color;
        initColor = new Color(c.r,c.g,c.b,1.0f);
        menuController = gameObject.transform.root.GetComponent<MenuController>();
        audio = GetComponent<CityAudio>();

        menuController.MenuType
            .Where(mType => mType == MenuType.StageSelect2 && isTransparent)
            .Subscribe(_ =>
            {
                StartCoroutine(ButtonFadeInRoutine(FADE_IN_TIME));
            });
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        image.color = downColor;
        audio.ShotAudio();
        if (!isTransparent)
        {
            StartCoroutine(ButtonFadeOutRoutine(FADE_OUT_TIME,true));
            backButton.FadeOut(false);
            sep.FadeOut();
            foreach (var b in lockedButtons)
            {
                b.FadeOut(false);
            }
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

    private IEnumerator ButtonFadeOutRoutine(float time, bool isCity)
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
        if (isCity)
        {
            menuController.MenuType.Value = MenuType.YesNoGameStart;
        }
    }

    public void FadeOut(bool isCity)
    {
        StartCoroutine(ButtonFadeOutRoutine(FADE_OUT_TIME,isCity));
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