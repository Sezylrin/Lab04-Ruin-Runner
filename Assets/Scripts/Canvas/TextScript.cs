using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text text;
    void Start()
    {
        text = gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.GetComponent<RectTransform>().localScale = new Vector2(1.1f, 1.1f);
        text.color = HelperFunctions.GetColorFromHex("#dedede");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
        text.color = Color.white;
    }
}
