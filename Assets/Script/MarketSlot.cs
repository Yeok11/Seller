using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MarketSlot : SingleTon<MarketSlot>, IPointerDownHandler
{
    public Transform InfoDataPos;

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

    public void TextUpdate()
    {
        int price = Mathf.RoundToInt(DataManager.Instance.ItemPrice[PosNum] - (DataManager.Instance.ItemPrice[PosNum] * DataManager.Instance.BonusPer[2, DataManager.Instance.InteriorLevel[2] - 1] / 100) - Mathf.RoundToInt(DataManager.Instance.ItemPrice[PosNum] * SubSystemManager.Instance.PrimarySale));

        price = Mathf.RoundToInt(price / 10) * 10;

        transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().SetText(price.ToString());
    }

    void Start()
    {
        if (PosNum == 0)
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
        
        TextUpdate();
    }
}
