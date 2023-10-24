using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubSystemManager : SingleTon<SubSystemManager>
{
    GameObject BtList_Obj;

    [SerializeField] GameObject OrderItemType;
    [SerializeField] TextMeshProUGUI MarketSchedule;

    [SerializeField] private Transform BagContants;

    private void Awake()
    {
        BtList_Obj = GameObject.Find("BtList");
        BtList_Obj.SetActive(false);
    }

    private void Start()
    {
        BagItemReset();
    }

    public void BtTurn(int BtType)
    {
        GameObject _BtList;

        if (BtType != 4) _BtList = BtList_Obj.transform.GetChild(BtType).GetChild(0).gameObject;
        else { _BtList = BtList_Obj; }

        if (_BtList.activeSelf == false) SubWindowOff(_BtList);
        else { _BtList.SetActive(false); }
    }

    
    public void SubWindowOff(GameObject OpenObject)
    {
        BtList_Obj.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        BtList_Obj.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        BtList_Obj.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        BtList_Obj.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);

        if (OpenObject != null) OpenObject.SetActive(true);
        if (OpenObject.name == "MarketWindow") MarketSlot.Instance.InfoDataPos.parent.gameObject.SetActive(false);
        if (OpenObject.name == "BagWindow") BagItemReset();
    }

    private void BagItemReset()
    {
        for (int i = 0; i < 15; i++)
        {
            BagContants.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.Instance.ItemType[i];
            BagContants.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "X " + DataManager.Instance.ItemCount[i];
            BagContants.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = DataManager.Instance.ItemPrice[i].ToString();
        }
    }












    //상점 세부 창
    private void MarketCntSign()
    {
        DataManager.Instance.MarketBtsPos.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = DataManager.Instance.MarketItemCnt.ToString();
    }

    internal void MarketScheduleUpdate()
    {
        MarketSchedule.text = $"다음 상인 방문까지 : {ScheduleJudge()}일";
    }

    private int ScheduleJudge()
    {
        if (DataManager.Instance.Days % 7 < 3)
        {
            return 3 - DataManager.Instance.Days % 7;
        }
        else
        {
            return 10 - DataManager.Instance.Days % 7;
        }
    }

    public void MarketCntUp(bool Ctrl)
    {
        if (Ctrl == true && DataManager.Instance.MarketItemCnt != 99) DataManager.Instance.MarketItemCnt += 1;
        else if (Ctrl == false && DataManager.Instance.MarketItemCnt != 0) DataManager.Instance.MarketItemCnt -= 1;
        
        MarketCntSign();
    }

    public void MarketBuyItem()
    {
        
        int j = 0;
        for (int i = 0; i < DataManager.Instance.ItemTypeCount; i++)
        {
            if (OrderItemType.GetComponent<TextMeshProUGUI>().text == DataManager.Instance.ItemType[i])
            {
                j = i;
                break;
            }
        }

        if (DataManager.Instance.ItemPrice[j] * DataManager.Instance.MarketItemCnt <= DataManager.Instance.HaveMoney)
        {
            Debug.Log("주문을 완료했습니다.");

            while (true)
            {
                DataManager.Instance.OrderData.Add(j);
                DataManager.Instance.MarketItemCnt--;

                if (DataManager.Instance.MarketItemCnt <= 0) break;
            }
        }
        else
        {
            Debug.Log("보유한 돈이 부족합니다.");
        }

        MarketCntReset();
    }

    internal void MarketCntReset()
    {
        DataManager.Instance.MarketItemCnt = 0;
        MarketCntSign();
    }
}
