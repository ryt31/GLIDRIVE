using System.Collections;
using Glidrive.Manager;
using Glidrive.Util;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchToStartButton : BaseButtonTransitioner
{
    private Image image;
    private Color initColor;
    private MenuController menuController;
    private TouchToStartAudio audio;
    private bool isTransparent = false;
    private const float FADE_OUT_TIME = 1.0f;
    private const float FADE_IN_TIME = 1.0f;
    
    // TODO 良くない...
    [SerializeField] private ExitButton exitButton;

    private void Start()
    {
        image = GetComponent<Image>();
        initColor = image.color;
        menuController = gameObject.transform.root.GetComponent<MenuController>();
        audio = GetComponent<TouchToStartAudio>();

        menuController.MenuType
            .Where(mType => mType == MenuType.TouchToStart && isTransparent)
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
            exitButton.FadeOut(false);
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

    private IEnumerator ButtonFadeOutRoutine(float time, bool isTouchToStart)
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
        if (isTouchToStart)
        {
            menuController.MenuType.Value = MenuType.StageSelect;
        }
    }

    public void FadeOut(bool isTouchToStart)
    {
        StartCoroutine(ButtonFadeOutRoutine(FADE_OUT_TIME,isTouchToStart));
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