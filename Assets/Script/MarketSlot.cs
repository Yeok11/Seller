using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MarketSlot : SingleTon<MarketSlot>, IPointerDownHandler
{
    [SerializeField] internal Transform InfoDataPos;

    internal int PosNum;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (InfoDataPos.parent.gameObject.activeSelf == false)
        {
            InfoDataPos.parent.gameObject.SetActive(true);
            ItemInfoDataSet();
        }
        else if (InfoDataPos.parent.gameObject.activeSelf == true)
        {
            if (InfoDataPos.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text == transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text)
            {
                InfoDataPos.parent.gameObject.SetActive(false);
            }
            else
            {
                ItemInfoDataSet();
            }
        }
    }

    private void ItemInfoDataSet()
    {
        InfoDataPos.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text;
        InfoDataPos.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text;
        InfoDataPos.GetChild(1).GetComponent<Image>().sprite = transform.GetChild(1).GetComponent<Image>().sprite;

        SubSystemManager.Instance.MarketCntReset();
    }

    private void Update()
    {
        TextUpdate();
    }

    void TextUpdate()
    {
        transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = DataManager.Instance.ItemPrice[PosNum].ToString();
    }

    void Start()
    {
        for (int i = 0; i < DataManager.Instance.ItemTypeCount; i++)
        {
            if (transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text == DataManager.Instance.ItemType[i])
            {
                PosNum = i;
                break;
            }
        }
    }
}
