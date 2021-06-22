using System.Collections;
using Glidrive.Manager;
using Glidrive.Util;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSelectImage : MonoBehaviour
{
    private Image image;
    private MenuController menuController;
    private bool isTransparent = true;
    private const float FADE_OUT_TIME = 1.0f;
    private const float FADE_IN_TIME = 1.0f;

    private void Start()
    {
        image = GetComponent<Image>();
        menuController = gameObject.transform.root.GetComponent<MenuController>();

        menuController.MenuType
            .Where(mType => mType == MenuType.StageSelect && isTransparent)
            .Subscribe(_ =>
            {
                StartCoroutine(ButtonFadeInRoutine(FADE_IN_TIME));
            });
    }

    private IEnumerator ButtonFadeOutRoutine(float time)
    {
        var elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            var color = image.color;
            image.color = new Color(color.r, color.g, color.b, Util.Map(elapsedTime, 0.0f, 1.0f, 1.0f, 0.0f));
            yield return null;
        }
        menuController.MenuType.Value = MenuType.StageSelect2;
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
        StartCoroutine(ButtonFadeOutRoutine(FADE_OUT_TIME));
    }
}