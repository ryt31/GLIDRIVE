using System.Collections;
using Glidrive.Util;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExitButton : BaseButtonTransitioner
{
    private const float FADE_OUT_TIME = 1.0f;
    private const float FADE_IN_TIME = 1.0f;

    // TODO 良くない...
    [SerializeField] private TouchToStartButton touchToButton;

    private Image image;
    private Color initColor;
    private bool isTransparent;
    private MenuController menuController;
    private ExitAudio audio;

    private void Start()
    {
        image = GetComponent<Image>();
        initColor = image.color;
        menuController = gameObject.transform.root.GetComponent<MenuController>();
        audio = GetComponent<ExitAudio>();

        menuController.MenuType
            .Where(mType => mType == MenuType.TouchToStart && isTransparent)
            .Subscribe(_ => { StartCoroutine(ButtonFadeInRoutine(FADE_IN_TIME)); });
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        image.color = downColor;
        image.raycastTarget = false;
        audio.ShotAudio();
        if (!isTransparent)
        {
            FadeOut(true);
            touchToButton.FadeOut(false);
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

    private IEnumerator ButtonFadeOutRoutine(float time, bool isExit)
    {
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
        if (isExit) menuController.MenuType.Value = MenuType.YesNoExit;
    }

    public void FadeOut(bool isExit)
    {
        StartCoroutine(ButtonFadeOutRoutine(FADE_OUT_TIME, isExit));
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

        image.raycastTarget = true;
        isTransparent = false;
    }
}