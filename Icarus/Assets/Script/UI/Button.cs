using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ButtonTextColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TMP_Text text;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color clickColor = Color.red;

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        text.color = clickColor;
    }
}