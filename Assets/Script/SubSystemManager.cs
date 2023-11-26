using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;
using EasyJson;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class SubSystemManager : SingleTon<SubSystemManager>
{
    internal GamePlayData RecordData = new GamePlayData();

    [SerializeField] internal GameObject BtList_Obj;

    [SerializeField] private GameObject OrderItemType;
    [SerializeField] private TextMeshProUGUI MarketSchedule;

    [SerializeField] private Transform BagContants;
    [SerializeField] private Transform InteriorLists;

    [SerializeField] private Transform MarketDataPos;
    [SerializeField] private GameObject MarketItemInfo;

   
    //internal int WeekBuyBonus;

    internal float PrimarySale;

    internal bool CanUseBt = false;

    internal float SalePer()
    {
        switch (DataManager.GameDif)
        {
            case Diff.Easy:
                return 0.3f;

            case Diff.Normal:
                return 0.25f;

            case Diff.Hard:
                return 0.15f;

            case Diff.Event_1:
                return 0.5f;

            default:
                return 0;
        }
    }

    private void Start()
    {
        PrimarySale = SalePer();
        

        if (TitleManager.ContinueData) DataLoad();

        if (BtList_Obj != null)
        {
            BtList_Obj.SetActive(false);

            if (DataManager.GameDif == Diff.Event_1)
            {
                for (int i = 0; i < DataManager.Instance.InteriorLevel.Length; i++)
                {
                    DataManager.Instance.InteriorLevel[i] = 5;
                }
            }

            CounterManager.Instance.ShelfReset();
            InteriorUpdate();
            TimeManager.Instance.NewDay();
        }
    }

    public void DataReset()
    {
        RecordData.M_Day = 0;
        RecordData.M_Money = 0;
        
        EasyToJson.ToJson(RecordData, "RecordPlay", true);
        TitleManager.ContinueData = false;
    }
    private void DataLoad()
    {
        RecordData = EasyToJson.FromJson<GamePlayData>("RecordPlay");
        DataManager.Instance.Days = RecordData.M_Day - 1;
        DataManager.Instance.HaveMoney = RecordData.M_Money;
        DataManager.Instance.ItemCount = RecordData.M_ItemCount;
        DataManager.Instance.InteriorLevel = RecordData.M_Interior;
        for (int i = 0; i < DataManager.Instance.UseGold.Count; i++)
        {
            DataManager.Instance.UseGold[i]         =     RecordData.M_Record[i];
            DataManager.Instance.BuyGold[i]         =     RecordData.M_Record[i+3];
            DataManager.Instance.SellCnt[i]         =     RecordData.M_Record[i+6];
            DataManager.Instance.ComeCustomerCnt[i] =     RecordData.M_Record[i+9];
            DataManager.Instance.MissCnt[i]         =     RecordData.M_Record[i+12];
            DataManager.Instance.BuyCnt             =     RecordData.M_Record[15];
        }
        TimeManager.Instance.EventDayContinue = RecordData.M_WeekEventTerm;
        TimeManager.Instance.WeekBonusValue[0] = RecordData.M_WeekBonus[1];
        TimeManager.Instance.WeekBonusValue[1] = RecordData.M_WeekBonus[2];
        for (int i = 1; i < TimeManager.Instance.WeekEventBoolTrigger.Length; i++) TimeManager.Instance.WeekEventBoolTrigger[i] = RecordData.M_WeekBonus[2 + i] == 1 ? true : false;
        TimeManager.Instance.WeekEventBoolTrigger[0] = RecordData.SpecialWeekEvent;
        TimeManager.Instance.DayMesCode = RecordData.M_DayMesCode;
        DataManager.Instance.MarketOrderData = RecordData.DeliveryData;
        DataManager.GameDif = RecordData.diff;
    }

    //전체 설정
    public void BtTurn(int BtType)
    {
        if (CanUseBt == true)
        {
            GameObject _BtList;

           
            if (BtType != 4) _BtList = BtList_Obj.transform.GetChild(BtType).GetChild(0).gameObject;
            else { _BtList = BtList_Obj; }

            if (_BtList.activeSelf == false) SubWindowOff(_BtList);
            else { _BtList.SetActive(false); }
            /*
            if (DataManager.GameDif != Diff.Event_1)
            {
                if (_BtList.activeSelf == false) SubWindowOff(_BtList);
                else { _BtList.SetActive(false); }
            }
            else
            {
                if (BtType == 0 || BtType == 4)
                {
                    if (_BtList.activeSelf == false) SubWindowOff(_BtList);
                    else { _BtList.SetActive(false); }
                }
            }
            */
        }
    }
    public void SubWindowOff(GameObject OpenObject)
    {
        try
        {
            BtList_Obj.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            BtList_Obj.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            BtList_Obj.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            BtList_Obj.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
        }
        catch
        {

        }
        

        if (OpenObject != null) OpenObject.SetActive(true);
        if (OpenObject.name == "MarketWindow")
        {
            MarketItemInfo.gameObject.SetActive(false);
            OpenObject.transform.GetChild(0).GetComponentInChildren<Scrollbar>().value = 1;
        }
        if (OpenObject.name == "BagWindow") BagItemReset();
    }

    //창고
    private void BagItemReset()
    {
        for (int i = 0; i < 15; i++)
        {
            BagContants.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.Instance.ItemType[i];
            BagContants.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "X " + DataManager.Instance.ItemCount[i];
            BagContants.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = DataManager.Instance.ItemPrice[i].ToString();
        }
    }

    //인테리어
    public void InteriorUpgrade(int j)
    {
        if (DataManager.Instance.NextCost[DataManager.Instance.InteriorLevel[j] -1, j] <= DataManager.Instance.HaveMoney && DataManager.Instance.InteriorLevel[j] < 5)
        {
            DataManager.Instance.HaveMoney -= DataManager.Instance.NextCost[DataManager.Instance.InteriorLevel[j] -1, j];
            DataManager.Instance.InteriorLevel[j]++;
            InteriorUpdate();
        }
        else
        {
            Debug.Log("잔액이 부족합니다.");
        }
    }
    private void InteriorUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            if (DataManager.Instance.InteriorLevel[i] < 5)
            {
                InteriorLists.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Lv." + DataManager.Instance.InteriorLevel[i];
            }
            else
            {
                InteriorLists.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Lv.Max";
            }
            InteriorLists.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = DataManager.Instance.InteriorLevel[i] == 5 ? "---------  " :  DataManager.Instance.NextCost[DataManager.Instance.InteriorLevel[i]-1, i].ToString();
        }

        InteriorLists.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = $"영업 시간 {DataManager.Instance.BonusPer[0,(DataManager.Instance.InteriorLevel[0] - 1)]}% 연장";
        InteriorLists.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = $"손님 방문속도 {DataManager.Instance.BonusPer[1, (DataManager.Instance.InteriorLevel[1] - 1)]}% 상승";
        InteriorLists.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = $"전 제품 매입가 {DataManager.Instance.BonusPer[2, (DataManager.Instance.InteriorLevel[2] - 1)]}% 감소";
        InteriorLists.GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = $"전 제품 판매가 {DataManager.Instance.BonusPer[3, (DataManager.Instance.InteriorLevel[3] - 1)]}% 상승";
    }

    #region 상점
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
        if (DataManager.Instance.Days % 7 < 3) return 3 - DataManager.Instance.Days % 7;
        else return 10 - DataManager.Instance.Days % 7;
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

        //매입가 = 판매가 - 인테리어 세일값 - 기본 매입 감소
        int Sale = DataManager.Instance.ItemPrice[j] - (DataManager.Instance.ItemPrice[j] * DataManager.Instance.BonusPer[2, DataManager.Instance.InteriorLevel[2] - 1] / 100) 
            - Mathf.RoundToInt(DataManager.Instance.ItemPrice[j] * PrimarySale);

        if (Sale * DataManager.Instance.MarketItemCnt <= DataManager.Instance.HaveMoney)
        {
            Debug.Log($"주문을 완료했습니다. -{Sale} X {DataManager.Instance.MarketItemCnt}");

            DataManager.Instance.UseGold[2] += Sale * DataManager.Instance.MarketItemCnt;

            while (true)
            {
                if (DataManager.Instance.MarketItemCnt <= 0) break;

                DataManager.Instance.BuyCnt++;
                DataManager.Instance.MarketOrderData.Add(j);
                DataManager.Instance.HaveMoney -= Sale;
                DataManager.Instance.MarketItemCnt--;
            }

            MarketCntReset();
        }
        else
        {
            Debug.Log("보유한 돈이 부족합니다.");
        }
    }
    internal void MarketCntReset()
    {
        DataManager.Instance.MarketItemCnt = 0;
        MarketCntSign();
    }
    #endregion;

    //설정
    public void Title(bool End)
    {
        if (!End)
        {
            RecordData.M_Day = DataManager.Instance.Days;
            RecordData.M_Money = DataManager.Instance.HaveMoney;
            RecordData.M_ItemCount = DataManager.Instance.ItemCount;
            RecordData.M_Interior = DataManager.Instance.InteriorLevel;
            for (int i = 0; i < DataManager.Instance.UseGold.Count; i++)
            {
                RecordData.M_Record[i] = DataManager.Instance.UseGold[i];
                RecordData.M_Record[i+3] = DataManager.Instance.BuyGold[i];
                RecordData.M_Record[i+6] = DataManager.Instance.SellCnt[i];
                RecordData.M_Record[i+9] = DataManager.Instance.ComeCustomerCnt[i];
                RecordData.M_Record[i+12] = DataManager.Instance.MissCnt[i];
                RecordData.M_Record[15] = DataManager.Instance.BuyCnt;
            }

            //CounterManager.Instance.WeekSellBonus = (int a) => RecordData.M_WeekBonus[1] = a; ;
            //WeekBuyBonus = (int a) => RecordData.M_WeekBonus[2] = a; ;

            RecordData.M_WeekEventTerm = TimeManager.Instance.EventDayContinue;
            RecordData.M_WeekBonus[1] = TimeManager.Instance.WeekBonusValue[0];
            RecordData.M_WeekBonus[2] = TimeManager.Instance.WeekBonusValue[1];
            for (int i = 1; i < TimeManager.Instance.WeekEventBoolTrigger.Length; i++) RecordData.M_WeekBonus[2 + i] = TimeManager.Instance.WeekEventBoolTrigger[i] ? 1 : 0;
            RecordData.SpecialWeekEvent = TimeManager.Instance.WeekEventBoolTrigger[0];
            RecordData.DeliveryData = DataManager.Instance.MarketOrderData;

            RecordData.M_DayMesCode = TimeManager.Instance.DayMesCode;
            RecordData.diff = DataManager.GameDif;

            EasyToJson.ToJson(RecordData, "RecordPlay", true);
            TitleManager.ContinueData = true;
        }

        SceneManager.LoadScene("Title");
    }
    public void GameOff()
    {
        Application.Quit();
    }
}
[System.Serializable]
class GamePlayData
{
    public int M_Day;
    public int M_Money;
    public int[] M_ItemCount;
    public int[] M_Interior;
    public int[] M_Record = new int[20];
    public int[] M_WeekBonus = new int[20]; //{ 일수, 판매가, 구매가, 귀족 발길 여부, 소리 } 
    public int[] M_WeekEventTerm;
    public bool SpecialWeekEvent;
    public int M_DayMesCode;
    public List<int> DeliveryData;
    public Diff diff;
}
