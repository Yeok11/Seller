using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Maker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private void OnEnable()
    {
        GetComponentInChildren<TextMeshProUGUI>().SetText("������");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponentInChildren<TextMeshProUGUI>().SetText("shy");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponentInChildren<TextMeshProUGUI>().SetText("������");
    }
}
