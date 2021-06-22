using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseButtonTransitioner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    // ボタンの各種色設定
    public Color32 normalColor = Color.white;
    public Color32 hoverColor = Color.grey;
    public Color32 downColor = Color.white;

    public abstract void OnPointerClick(PointerEventData eventData); // ボタンをクリックしたとき
    public abstract void OnPointerDown(PointerEventData eventData); // ボタンを押したとき
    public abstract void OnPointerEnter(PointerEventData eventData); // ボタンにホバーしたとき   
    public abstract void OnPointerExit(PointerEventData eventData); // ボタンから出たとき
    public abstract void OnPointerUp(PointerEventData eventData); // ボタンを押した状態で離したとき
}